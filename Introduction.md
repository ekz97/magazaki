# Inleiding

- BeneficiaryAPI is de "SHOP"
- FinancialInstituteAPI is de "BANK"
- PeasieAPI is de "man in the middle" (Pay Easily)
- Peasie.Identity.App is een Microsoft Identity gebaseerde app (Razor) met een mogelijkheid om met emailbevestiging als gebruiker te registeren en met integratie van Google login en ondersteuning voor 2FA (bijvoorbeeld Microsoft Authenticator)

# Wat moet ik nog doen?

- Peasie.Identity.App uitbreiden zodat er eenmalig een "Peasie" public key code wordt aangeboden waarmee geauthenticeerd en geauthorizeerd kan worden bij Peasie; de private key moet onleesbaar in de db komen,
  toegankelijk voor Peasie.Identity.App en PeasieAPI; de public key moet veilig bewaard worden door de gebruiker (BeneficiaryAPI en FinancialInstituteAPI). Doel: HvA kan Microsoft Identity eens aan de tand voelen ;-)
- een "callback" via web api verder implementeren van FinancialInstituteAPI naar de BeneficiaryAPI over PeasieAPI. De bank kan dan op eigen initiatief iets over het verwerken van de betaling laten weten aan de aanvrager
  van de betaling.
- uitvissen of er nog sterkere asymmetrische encryptie mogelijk is in .NET
- de private/public key coding met encryptie / decryptie tussen de partijen veralgemenen zodat de code overzichtelijker en algemener wordt

# Wat kan Jeroen doen (op termijn)?

- de code leren kennen en een mooie presentatie (PowerPoint) maken van de werking (zie hieronder "overzicht flow")
- een beperkte "audit trail" (wat is er uitgevoerd door PeasieAPI) opslaan in de databank; deze audit trail mag een configureerbaar "window" zijn, bijvoorbeeld laatste week, zodat de databank niet explodeert
- de code beheren
- eerst Nestor en daarna de andere teams laten integreren door hen te helpen de basis van de BeneficiaryAPI (Nestrix) en FinancialInstituteAPI (Magazaki) in te bouwen in hun backends
- de razor client van Peasie.Identity.App uitbreiden met een "dashboard" waarin de gebruiker zijn eigen "Peasie" public key eventueel kan herstellen
- opvolgen of de tokens en sessies overheen de vele uren ok blijven
- de betaling verfijnen (stadia bank met veel latere callback, maar is eerder iets voor team Nestrix)

# Overzicht flow

- in appsettings.json van de belangrijkste rolspelers (BeneficiaryAPI en FinancialInstituteAPI) staat een attribuut "DemoMode"; dit zorgt ervoor dat beide automatisch:

- een geencrypteerd authenticatie token aanvragen bij PeasieAPI (voor "Bearer")
- een sessie aanvragen bij PeasieAPI; hierin wordt een public key beschikbaar gesteld waarmee de communicatie in de sessie verder geencrypteerd kan verlopen; de client stuurt meer details over zichzelf
- de sessie wordt via HangFire automatisch geverifieerd elke minuut en in principe opnieuw aangelegd indien dit niet het geval is; dit kunnen we uitbreiden tot effectief opnieuw aanleggen, bijvoorbeeld elk uur (te testen)

Elke minuut wordt als demo een betaling gevraagd vanuit de BeneficiaryAPI naar de FinancialInstituteAPI; hierbij wordt niet gecontroleerd of er voldoende provisie op de rekening staat - er wordt gewoon teruggestuurd
dat de betaling verwerkt is; eerst wordt een PaymentID gevraagd en hiervoor wordt een tijdelijke dynamische private/public key combinatie voorzien op een dergelijke manier dat PeasieAPI geen toegang heeft om de communicatie 
tussen de BeneficiaryAPI en FinancialInstituteAPI te bekijken en te wijzigen. Op dit moment wordt zelfs het bericht van de bank naar de shop via de private session key voor de bank gedecrypteerd in PeasieAPI en met de public
session key van de shop geencrypteerd in PeasieAPI bij het versturen naar BeneficiaryAPI (deze stap zouden we bijvoorbeeld kunnen overslaan)


