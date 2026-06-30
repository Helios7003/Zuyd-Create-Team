using FestivalAppv2.Data;
using FestivalAppv2.Models;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Het wachtwoord blijft buiten GitHub. Het wordt op de VM via systemd als
// omgevingsvariabele DATABASE_PASSWORD aan de applicatie gegeven.
string? databaseWachtwoord = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

if (string.IsNullOrWhiteSpace(databaseWachtwoord))
{
    throw new InvalidOperationException(
        "DATABASE_PASSWORD ontbreekt. Stel deze omgevingsvariabele in voordat de webapp start.");
}

// De bestaande repositoryklassen worden hergebruikt.
builder.Services.AddSingleton(new ArtiestRepository(databaseWachtwoord));
builder.Services.AddSingleton(new OptredenRepository(databaseWachtwoord));

var app = builder.Build();

// Nginx draait op dezelfde VM en geeft deze headers door.
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeadersOptions.KnownProxies.Add(IPAddress.Loopback);
app.UseForwardedHeaders(forwardedHeadersOptions);

// Deze endpoint wordt gebruikt voor lokale controles en de Azure Load Balancer health probe.
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "ok",
    application = "Vallis Nexus Festival"
}));

// ===== ARTIESTEN =====

app.MapGet("/api/artiesten", (ArtiestRepository artiestRepository) =>
{
    var artiesten = artiestRepository.GetAlleArtiesten()
        .Select(artiest => new
        {
            artiestId = artiest.GetArtiestId(),
            naam = artiest.GetArtiestnaam(),
            genre = artiest.GetGenre()
        });

    return Results.Ok(artiesten);
});

app.MapPost("/api/artiesten", (ArtiestInvoer invoer, ArtiestRepository artiestRepository) =>
{
    if (string.IsNullOrWhiteSpace(invoer.Naam) || string.IsNullOrWhiteSpace(invoer.Genre))
    {
        return Results.BadRequest(new { fout = "Naam en genre zijn verplicht." });
    }

    var artiest = new Artiest(0, invoer.Naam.Trim(), invoer.Genre.Trim());
    artiestRepository.VoegArtiestToe(artiest);

    return Results.Created("/api/artiesten", new
    {
        bericht = "Artiest is toegevoegd."
    });
});

app.MapPut("/api/artiesten/{id:int}", (int id, ArtiestInvoer invoer, ArtiestRepository artiestRepository) =>
{
    if (string.IsNullOrWhiteSpace(invoer.Naam) || string.IsNullOrWhiteSpace(invoer.Genre))
    {
        return Results.BadRequest(new { fout = "Naam en genre zijn verplicht." });
    }

    var artiest = new Artiest(id, invoer.Naam.Trim(), invoer.Genre.Trim());
    artiestRepository.WijzigArtiest(artiest);

    return Results.Ok(new { bericht = "Artiest is gewijzigd." });
});

app.MapDelete("/api/artiesten/{id:int}", (int id, ArtiestRepository artiestRepository) =>
{
    artiestRepository.VerwijderArtiest(id);
    return Results.NoContent();
});

// ===== PROGRAMMA / OPTREDENS =====

app.MapGet("/api/optredens", (
    string? artiest,
    string? podium,
    OptredenRepository optredenRepository) =>
{
    IEnumerable<Optreden> optredens = optredenRepository.GetAlleOptredens();

    if (!string.IsNullOrWhiteSpace(artiest))
    {
        optredens = optredens.Where(optreden =>
            optredensNaamBevat(optreden.GetArtiest().GetArtiestnaam(), artiest));
    }

    if (!string.IsNullOrWhiteSpace(podium))
    {
        optredens = optredens.Where(optreden =>
            optredensNaamBevat(optreden.GetPodium().GetPodiumnaam(), podium));
    }

    var resultaat = optredens.Select(optreden => new
    {
        optredenId = optreden.GetOptredenId(),
        datum = optreden.GetDatum().ToString("yyyy-MM-dd"),
        starttijd = optreden.GetStarttijd().ToString(@"hh\:mm"),
        eindtijd = optreden.GetEindtijd().ToString(@"hh\:mm"),
        artiest = optredensNaam(optreden.GetArtiest().GetArtiestnaam()),
        podium = optredensNaam(optreden.GetPodium().GetPodiumnaam())
    });

    return Results.Ok(resultaat);
});

app.MapGet("/", () => Results.Content(GetWebpagina(), "text/html; charset=utf-8"));

app.Run();

static bool optredensNaamBevat(string waarde, string zoekterm)
{
    return waarde.Contains(zoekterm, StringComparison.OrdinalIgnoreCase);
}

static string optredensNaam(string waarde)
{
    return waarde ?? string.Empty;
}

static string GetWebpagina() => """
<!doctype html>
<html lang="nl">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Vallis Nexus Festival</title>
  <style>
    :root { color-scheme: light; font-family: Arial, sans-serif; }
    body { margin: 0; background: #f4f6f8; color: #18212b; }
    header { background: #173a5e; color: white; padding: 28px 7%; }
    header h1 { margin: 0 0 6px; }
    header p { margin: 0; opacity: .9; }
    main { width: min(1100px, 86%); margin: 28px auto 50px; }
    .grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(320px, 1fr)); gap: 22px; }
    section { background: white; border-radius: 10px; padding: 22px; box-shadow: 0 2px 10px #0b1a2814; }
    h2 { margin-top: 0; }
    form, .filters { display: grid; gap: 10px; }
    input { padding: 10px; border: 1px solid #cbd4df; border-radius: 6px; font: inherit; }
    button { border: 0; border-radius: 6px; padding: 10px 14px; cursor: pointer; font: inherit; background: #173a5e; color: white; }
    button.secondary { background: #5d6874; }
    button.danger { background: #a32626; }
    table { width: 100%; border-collapse: collapse; margin-top: 14px; }
    th, td { text-align: left; padding: 10px; border-bottom: 1px solid #e2e7ed; vertical-align: top; }
    th { color: #40505e; font-size: .9rem; }
    .actions { display: flex; gap: 6px; flex-wrap: wrap; }
    #melding { min-height: 22px; margin: 14px 0; font-weight: 600; }
    .ok { color: #156b35; } .error { color: #a32626; }
    .wide { grid-column: 1 / -1; }
    @media (max-width: 650px) {
      main { width: min(94%, 1100px); }
      table { font-size: .88rem; }
      th, td { padding: 7px 4px; }
    }
  </style>
</head>
<body>
  <header>
    <h1>Vallis Nexus Festival</h1>
    <p>Beheer artiesten en bekijk het festivalprogramma.</p>
  </header>

  <main>
    <div id="melding" role="status"></div>

    <div class="grid">
      <section>
        <h2 id="formulierTitel">Artiest toevoegen</h2>
        <form id="artiestForm">
          <input id="artiestId" type="hidden">
          <label>Naam
            <input id="naam" required maxlength="100" placeholder="Bijvoorbeeld: The Nexus Band">
          </label>
          <label>Genre
            <input id="genre" required maxlength="100" placeholder="Bijvoorbeeld: Pop">
          </label>
          <div class="actions">
            <button type="submit">Opslaan</button>
            <button type="button" class="secondary" id="annuleren">Annuleren</button>
          </div>
        </form>
      </section>

      <section>
        <h2>Programma filteren</h2>
        <div class="filters">
          <label>Artiest
            <input id="zoekArtiest" placeholder="Zoek op artiest">
          </label>
          <label>Podium
            <input id="zoekPodium" placeholder="Filter op podium">
          </label>
          <div class="actions">
            <button id="filterKnop" type="button">Filter toepassen</button>
            <button id="resetKnop" class="secondary" type="button">Filters wissen</button>
          </div>
        </div>
      </section>

      <section class="wide">
        <h2>Artiesten</h2>
        <table>
          <thead><tr><th>ID</th><th>Naam</th><th>Genre</th><th>Acties</th></tr></thead>
          <tbody id="artiestenLijst"></tbody>
        </table>
      </section>

      <section class="wide">
        <h2>Festivalprogramma</h2>
        <table>
          <thead><tr><th>Datum</th><th>Tijd</th><th>Artiest</th><th>Podium</th></tr></thead>
          <tbody id="optredensLijst"></tbody>
        </table>
      </section>
    </div>
  </main>

  <script>
    const melding = document.getElementById("melding");

    function toonMelding(tekst, isFout = false) {
      melding.textContent = tekst;
      melding.className = isFout ? "error" : "ok";
    }

    function escapen(waarde) {
      return String(waarde ?? "").replace(/[&<>"']/g, teken => ({
        "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;"
      })[teken]);
    }

    async function laadArtiesten() {
      const antwoord = await fetch("/api/artiesten");
      if (!antwoord.ok) throw new Error("Artiesten konden niet worden opgehaald.");
      const artiesten = await antwoord.json();
      const lijst = document.getElementById("artiestenLijst");

      lijst.innerHTML = artiesten.length
        ? artiesten.map(a => `
            <tr>
              <td>${a.artiestId}</td>
              <td>${escapen(a.naam)}</td>
              <td>${escapen(a.genre)}</td>
              <td class="actions">
                <button class="secondary" data-bewerk-id="${a.artiestId}" data-bewerk-naam="${escapen(a.naam)}" data-bewerk-genre="${escapen(a.genre)}">Wijzigen</button>
                <button class="danger" data-verwijder-id="${a.artiestId}" data-verwijder-naam="${escapen(a.naam)}">Verwijderen</button>
              </td>
            </tr>`).join("")
        : '<tr><td colspan="4">Er zijn geen artiesten gevonden.</td></tr>';

      lijst.querySelectorAll("[data-bewerk-id]").forEach(knop => {
        knop.addEventListener("click", () => {
          bewerkArtiest(Number(knop.dataset.bewerkId), knop.dataset.bewerkNaam, knop.dataset.bewerkGenre);
        });
      });

      lijst.querySelectorAll("[data-verwijder-id]").forEach(knop => {
        knop.addEventListener("click", () => {
          verwijderArtiest(Number(knop.dataset.verwijderId), knop.dataset.verwijderNaam);
        });
      });
    }

    async function laadOptredens() {
      const artiest = document.getElementById("zoekArtiest").value.trim();
      const podium = document.getElementById("zoekPodium").value.trim();
      const parameters = new URLSearchParams();
      if (artiest) parameters.set("artiest", artiest);
      if (podium) parameters.set("podium", podium);

      const antwoord = await fetch("/api/optredens?" + parameters.toString());
      if (!antwoord.ok) throw new Error("Het programma kon niet worden opgehaald.");
      const optredens = await antwoord.json();
      const lijst = document.getElementById("optredensLijst");

      lijst.innerHTML = optredens.length
        ? optredens.map(o => `
            <tr>
              <td>${escapen(o.datum)}</td>
              <td>${escapen(o.starttijd)} – ${escapen(o.eindtijd)}</td>
              <td>${escapen(o.artiest)}</td>
              <td>${escapen(o.podium)}</td>
            </tr>`).join("")
        : '<tr><td colspan="4">Geen optredens gevonden.</td></tr>';
    }

    function resetFormulier() {
      document.getElementById("artiestId").value = "";
      document.getElementById("naam").value = "";
      document.getElementById("genre").value = "";
      document.getElementById("formulierTitel").textContent = "Artiest toevoegen";
    }

    function bewerkArtiest(id, naam, genre) {
      document.getElementById("artiestId").value = id;
      document.getElementById("naam").value = naam;
      document.getElementById("genre").value = genre;
      document.getElementById("formulierTitel").textContent = "Artiest wijzigen";
      window.scrollTo({ top: 0, behavior: "smooth" });
    }

    async function verwijderArtiest(id, naam) {
      if (!confirm(`Weet je zeker dat je ${naam} wilt verwijderen?`)) return;
      try {
        const antwoord = await fetch(`/api/artiesten/${id}`, { method: "DELETE" });
        if (!antwoord.ok) throw new Error("Verwijderen is mislukt.");
        toonMelding("Artiest is verwijderd.");
        await laadArtiesten();
      } catch (fout) {
        toonMelding(fout.message, true);
      }
    }

    document.getElementById("artiestForm").addEventListener("submit", async event => {
      event.preventDefault();
      const id = document.getElementById("artiestId").value;
      const gegevens = {
        naam: document.getElementById("naam").value.trim(),
        genre: document.getElementById("genre").value.trim()
      };

      try {
        const antwoord = await fetch(id ? `/api/artiesten/${id}` : "/api/artiesten", {
          method: id ? "PUT" : "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(gegevens)
        });

        const resultaat = await antwoord.json().catch(() => ({}));
        if (!antwoord.ok) throw new Error(resultaat.fout ?? "Opslaan is mislukt.");

        toonMelding(resultaat.bericht ?? "Opgeslagen.");
        resetFormulier();
        await laadArtiesten();
      } catch (fout) {
        toonMelding(fout.message, true);
      }
    });

    document.getElementById("annuleren").addEventListener("click", resetFormulier);
    document.getElementById("filterKnop").addEventListener("click", () => laadOptredens().catch(fout => toonMelding(fout.message, true)));
    document.getElementById("resetKnop").addEventListener("click", () => {
      document.getElementById("zoekArtiest").value = "";
      document.getElementById("zoekPodium").value = "";
      laadOptredens().catch(fout => toonMelding(fout.message, true));
    });

    Promise.all([laadArtiesten(), laadOptredens()])
      .catch(fout => toonMelding(fout.message, true));
  </script>
</body>
</html>
""";

public record ArtiestInvoer(string Naam, string Genre);
