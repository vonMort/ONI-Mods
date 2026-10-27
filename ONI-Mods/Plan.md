# Plan: Kompakte Smart Battery mit Rueckwandbefestigung

## Ziel und bestaetigter Umfang

Eine eigenstaendige ONI-Mod fuegt eine neue Batterie hinzu, ohne bestehende
Vanilla-Gebaeude zu veraendern:

- Abmessungen: 1 Feld breit und 2 Felder hoch.
- Grafik und Animationen: kleine, NICHT smarte Vanilla-Batterie verwenden.
  Diese besitzt bereits die passende 1x2-Bauform. Keine eigene Grafik im
  ersten Umfang.
- Forschung: gemeinsam mit der Vanilla Smart Battery freischalten.
- Verhalten: Smart Battery einschliesslich einstellbarer Automationsschwellen,
  Automation-Ausgang, Ladezustandsanzeige und zugehoeriger Benutzeroberflaeche.
- Speicherkapazitaet: exakt 50 % der Smart Battery der Zielspielversion.
  Bei einem Vanilla-Wert von 20 kJ ergibt das 10 kJ.
- Baukosten: 250 kg raffiniertes Metall; gleiche Materialauswahl wie bei
  der Smart Battery. Bestaetigt als +25 % gegenueber deren 200 kg.
- Befestigung: ausschliesslich an einer Rueckwand, ohne Bodenunterstuetzung.
  Das Platzierungs- und Stuetzerhaltungsverhalten der neuen aquatischen
  Pflegestation dient als Referenz.
- Alle sonstigen Werte und Attribute entsprechen der Smart Battery,
  insbesondere Waerme, Energieverlust, Bauzeit, Lebenspunkte, Dekor und
  Ueberhitzung sowie gegebenenfalls weitere relevante Komponenten.
- Unterstuetzung fuer Basisspiel und Spaced Out! ohne DLC-Pflicht fuer
  das neue Gebaeude.

## Ausgangslage

Projektwurzel: `/home/mo/RiderProjects/ONI-Mods`.

Vorhanden sind `ONI-Mods.sln`, `ONI-Mods/ONI-Mods.csproj` und
`ONI-Mods/Program.cs`. Das Projekt ist aktuell ein .NET-10-Konsolenprogramm
mit Hello-World-Einstiegspunkt, noch keine ONI-Mod. Im geoeffneten
Projekt wurde kein Git-Repository erkannt.

Die installierte ONI-Version, deren Assembly-Pfade und die konkrete
Rueckwand-API sind noch nicht ermittelt. API-Namen, Framework-Ziel,
Forschungs-ID und Animation-IDs werden deshalb nicht geraten, sondern
vor der Implementierung aus der Zielinstallation bestimmt.

## Vorgehen und Todos

### 1. Plan im Projekt ablegen und Zielinstallation erfassen

- Nach Planfreigabe diesen Plan als `PLAN.md` direkt in der Projektwurzel
  ablegen, wie angefordert.
- Installationspfad und Spielversion feststellen; falls nicht lokal
  ermittelbar, gezielt beim Nutzer erfragen.
- Vanilla-Konfigurationen der Smart Battery, kleinen Batterie und
  aquatischen Pflegestation sowie zugehoerige Komponenten untersuchen.
- Dabei Forschungseintrag, Baumenue, Materialkosten, alle Standardwerte,
  Grafik-/Animationsvertraege, Strom-/Automation-Ports und Rueckwandregeln
  erfassen. Vorhandene Modding-Konventionen der Installation beachten.
- Klaeren, welche Rueckwandtypen zaehlen, welche Zellen gestuetzt sein
  muessen und was beim Entfernen der Rueckwand passiert. Das entsprechende
  Vanilla-Verhalten uebernehmen, statt eigene Ersatzregeln zu erfinden.
- Keine Git-Initialisierung und keine Commits ohne gesonderten Auftrag.

### 2. Bestehendes Projekt in eine ladbare ONI-Mod ueberfuehren

- `ONI-Mods/ONI-Mods.csproj` auf eine Klassenbibliothek mit dem von ONI
  unterstuetzten Framework und passenden lokalen Assembly-Referenzen umstellen.
- `Program.cs` durch einen Mod-Einstiegspunkt ersetzen, soweit fuer die
  verwendete KMod-/Harmony-Integration erforderlich.
- Installationspfade lokal konfigurierbar halten, nicht als feste
  Entwicklerpfade im Quellcode hinterlegen.
- ONI-/Unity-/Harmony-Abhaengigkeiten aus der vorhandenen Spielinstallation
  verwenden; keine Spiel-DLLs in das Mod-Paket kopieren.
- `mod.yaml` und `mod_info.yaml` mit konsistentem Mod-Namen, Versionsdaten
  und tatsaechlich unterstuetztem Spiel-/DLC-Bereich ergaenzen.
- Minimalen Build-/Deployment-Hinweis und ein reproduzierbares Mod-Paket
  mit DLL und Metadaten vorsehen. Keine neue Options- oder UI-Bibliothek.

### 3. Neue Gebaeudekonfiguration implementieren

- Neue Konfiguration, voraussichtlich `CompactSmartBatteryConfig.cs`,
  mit eindeutiger Gebaeude-ID erstellen.
- Smart-Battery-Konfiguration und Komponenten als fachliche Referenz
  verwenden; vorhandene Initialisierung wiederverwenden, wo die API
  dies ohne Veraenderung des Vanilla-Prefabs ermoeglicht.
- Ausdrueckliche Abweichungen auf Groesse, Grafik, Kapazitaet, Baukosten
  und Befestigung begrenzen.
- Grafik der kleinen Batterie mit dem Smart-Battery-Verhalten verbinden.
  Vorhandene Animationsnamen und Symbolbindungen pruefen; falls sich
  die Animationsvertraege unterscheiden, eine kleine lokale Anpassung
  fuer das neue Gebaeude vorsehen, keine globalen Batterie-Patches.
- Kapazitaet am initialisierten Smart-Battery-Referenzwert orientieren
  und halbieren. Keine permanente Abhaengigkeit von einer Instanz des
  Vanilla-Gebaeudes erzeugen.
- Vorhandene Smart-Battery-Einstellungen und deren Speicherung verwenden;
  keine eigene Schwellenwertlogik nachbauen.

### 4. Rueckwandbefestigung und Anschluesse integrieren

- Vanilla-Rueckwandbefestigung der aquatischen Pflegestation auf das
  neue Gebaeude uebertragen, soweit im gemeinsamen Spielcode verfuegbar.
- Bodenpflicht entfernen und Rueckwandpflicht bei Vorschau, Errichtung
  und laufendem Betrieb konsistent anwenden.
- Keine Abhaengigkeit von einer DLC-spezifischen Pflegestationsklasse
  oder deren Assets erzeugen. Falls deren Helfer DLC-exklusiv ist,
  das entsprechende Verhalten mit den gemeinsamen Spielkomponenten
  implementieren und in beiden Spielvarianten pruefen.
- Stromanschluss und einen Automation-Ausgang auf gueltige Zellen innerhalb
  des 1x2-Grundrisses legen. Vanilla-Port-IDs und Signalsemantik beibehalten;
  konkrete Offsets anhand der kleinen Batterie und der Spiel-API bestimmen.
- Strom- und Automations-Overlay sowie Rueckwand-/Leitungs-Koexistenz
  pruefen. Kein zusaetzlicher Automation-Eingang.

### 5. Forschung, Baumenue und Texte registrieren

- Neue Batterie im Strom-Baumenue in der Naehe der Smart Battery eintragen.
- Zur selben Forschung wie die Smart Battery hinzufuegen, ohne deren
  vorhandene Freischaltungen zu ersetzen.
- Registrierungs-Patches eng auf die neue Gebaeude-ID begrenzen und
  doppelte Eintraege vermeiden.
- Namen, Beschreibung und Effekttexte auf Deutsch und Englisch anlegen;
  Kapazitaet, Automationsfunktion und Rueckwandpflicht klar ausweisen.
- Vorhandene Batterie-Symbole fuer Baumenue und Vorschau verwenden.
- Eindeutige Prefab-ID und persistente Einstellungen fuer Laden/Speichern
  sicherstellen.

### 6. Verhalten und Paket am Zielspiel pruefen

- Gegen die lokalen Spiel-Assemblies bauen und Mod-Ladevorgang kontrollieren.
- In Basisspiel und Spaced Out! jeweils folgende Kriterien pruefen:
  Forschung gemeinsam mit Smart Battery; Baumenue-Eintrag vorhanden;
  exakt 1x2 belegte Felder; kleine Vanilla-Batteriegrafik korrekt.
- Baukosten von 250 kg und unveraenderte raffinierte Materialauswahl pruefen.
- Platzierung an gueltiger Rueckwand ohne Boden erlauben; ohne gueltige
  Rueckwand auch bei vorhandenem Boden ablehnen. Verhalten bei
  Rueckwandentfernung mit der Vanilla-Referenz vergleichen.
- Laden/Entladen und exakt halbe Referenzkapazitaet messen; uebrige
  Werte und Attribute mit der Smart Battery vergleichen.
- Automation-Ausgang mit Leitung und Verbraucher verbinden; untere/obere
  Schwelle, Signalwechsel und Speicherverhalten dazwischen pruefen.
- Einstellungen, Ladezustand, Rueckwandbefestigung und Verkabelung nach
  Speichern/Laden pruefen; Vanilla-Batterien duerfen unveraendert bleiben.
- Vorhandene Testmoeglichkeiten nutzen. Falls keine automatisierten
  Spieltests existieren, die notwendigen Ingame-Schritte dokumentieren,
  statt ein neues Testframework einzufuehren.

## Geplante Dateien

- `PLAN.md` in der Projektwurzel nach Freigabe.
- `ONI-Mods/ONI-Mods.csproj`: Mod-Build und Referenzen.
- Ersatz fuer `ONI-Mods/Program.cs`: Mod-Einstiegspunkt, falls erforderlich.
- `ONI-Mods/CompactSmartBatteryConfig.cs`: Gebaeudedefinition und Komponenten.
- Eng begrenzte Registrierungs-Patches fuer Forschung und Baumenue.
- Lokalisierung, `mod.yaml`, `mod_info.yaml` und kurzer Build-/Installationshinweis.

Die genaue Ordnerstruktur folgt den beim Einrichten festgestellten
Projektkonventionen; keine unabhaengigen Architekturumbauten.

## Abgrenzung und bewusste Entscheidungen

- Keine eigenen Grafiken, Animationen, Sounds, neuen Forschungszweige,
  Optionen, zusaetzlichen Batteriegroessen oder Balance-Aenderungen.
- Nicht gleichzeitig auf Boden oder Rueckwand platzierbar: nur Rueckwand.
- Energieverlust und Waerme werden trotz halber Kapazitaet nicht halbiert.
  Damit bleibt die Vorgabe gleicher Vanilla-Werte erhalten; relativ zur
  gespeicherten Energie sind beide Werte entsprechend hoeher.
- Bestehende Gebaeude und Spielstaende werden nicht migriert.
- Fremdmods, die dieselben Vanilla-Referenzwerte veraendern, erhalten keine
  pauschale Kompatibilitaetsgarantie; keine spekulativen Kompatibilitaetspatches.
- Rueckwand-Mechanik und Animationen sind die zentralen technischen
  Pruefpunkte. Abweichungen vom bestaetigten Verhalten nicht stillschweigend
  vereinfachen, sondern vor Umsetzung mit dem Nutzer abstimmen.
