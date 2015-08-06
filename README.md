# YAHW - Yet Another Hardware Monitor

Ich möchte Euch hier einmal ein kleines Hobby-Projekt von mir vorstellen. In den letzten Woche habe ich mich ein wenig mit dem Thema "Hardware" auseinandergesetzt und in diesem Zusammenhang nach Software für die Hardwareüberwachung gesucht. Es gibt hier ja diverse Programm für die unterschiedlichsten Einsatzzwecke. Zum einen liefern manche Hardwarehersteller bei ihren Produkte direkt Software zur Systemüberwachung mit (z.B. ASUS mit der AI-Suite). Dies ist allerdings je nach Verwendungszweck mit einem riesigen Overhead verbunden. Möchte man z.B. nur einige Temperatursensoren überwachen ist man gezwungen das komplette Softwarepaket zu installieren, welches unter Umständen über 100 MB groß ist. Dann gibt es noch einige Freeware-Programme, die die Überwachung von Temperatursensoren, Lüftersteuerung, usw. übernehmen. Hier bin ich dann auf den <a href="http://openhardwaremonitor.org/" target="_blank">Open Hardware Monitor</a> gestoßen. Mit diesem Programm ist es möglich Temperaturen, Taktfrequenzen, Spannungen und Lüfter zu überwachen. Hier hat mir das UI allerdings nicht so gut gefallen und es fehlten mir einigen Informationen (z.B. HDD-Informationen, laufende Prozesse, usw.). Jetzt bieten die Entwickler des Open Hardware Monitors eine Bibliothek (DLL) für die Verwendung in eigenen Applikationen. Also habe ich mal auf dieser Basis angefangen eine kleine Applikation zu entwickeln welche ich hier nur einmal vorstellen möchte.

Das Ganze basiert auf dem .NET Framework 4.5.1 und nutzt als Oberflächentechnologie die Windows Presentation Foundation (WPF). Die Applikation selbst nutzt dabei die folgenden OpenSource-Projekte:
* <a href="http://openhardwaremonitor.org/" target="_blank">Open Hardware Monitor</a> zur Überwachung der diversen Hardwaresensoren (CPU, Temperatur, Spannungen, ...)
* <a href="https://github.com/firstfloorsoftware/mui" target="_blank">Modern UI for WPF (MUI)</a> als UI-Framework<li><a href="https://modernuicharts.codeplex.com/" target="_blank">Modern UI (Metro) Charts</a> zur Darstellung von Diagrammen
* <a href="http://oxyplot.org/" target="_blank">OxyPlot</a> zur Darstellung von Diagrammen
* und die Icons kommen von hier <a href="http://modernuiicons.com/" target="_blank">Modern UI Icons</a>

Die Applikation selbst befindet sich noch in einer sehr frühen Entwicklungsphase. Ziel der Applikation ist es die ermittelten Informationen auf möglichst übersichtliche Weise darzustellen. Hier mal einige Screenshots:

### Einstiegsseite

![Einstiegsseite](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_011.png)

### Informationen zum Mainboard

![Informationen zum Mainboard](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_021.png)

### Lüftersteuerung

![Lüftersteuerung](http://csharp-blog.de/wp-content/uploads/2015/08/YAHW_Fan_Controller.png)

### Informationen zur CPU (Auslastung, Temperaturen, Taktgeschwindigkeit)

![Informationen zur CPU (Auslastung, Temperaturen, Taktgeschwindigkeit)](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_041.png)

### Informationen zur Auslastung der einzelnen CPU-Cores

![Informationen zur Auslastung der einzelnen CPU-Cores](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_051.png)

### Temperaturverlauf der einzelnen CPU-Cores

![Temperaturverlauf der einzelnen CPU-Cores](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_061.png)

### Informationen zur verbauten Grafikkarte

![Informationen zur verbauten Grafikkarte](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_071.png)

### Auslastung des Arbeitsspeichers

![Auslastung des Arbeitsspeichers](http://csharp-blog.de/wp-content/uploads/2015/07/YAHW_081.png)

### S.M.A.R.T Informationen zu den verbauten Festplatten

![S.M.A.R.T Informationen zu den verbauten Festplatten](http://csharp-blog.de/wp-content/uploads/2015/08/YAHW_HDD_Info.png)

Aktuell sind noch nicht alle Dialoge fertig bzw. werden bestehende Dialoge nochmal überarbeitet. Zur Zeit gibt es noch die folgenden ToDos:

* Speichern der Anwendungseinstellungen (Farbschema, Sprache, ...)
* Logging im Fehlerfall
* Lüftersteuerung
* Alarme bei Überschreitung bestimmter Schwellwerte
* Tray-Icon
* Autostart
* ...
