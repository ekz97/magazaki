REM in directory Peasie, dus de root
copy .\localdvlp\PeasieAPI\appsettings.json .\Apps\PeasieAPI
copy .\localdvlp\PeasieAPI\Properties\launchSettings.json .\Apps\PeasieAPI\Properties\launchSettings.json
copy .\localdvlp\WebShopAPI\appsettings.json .\Magazaki\Apps\WebShopAPI
copy .\localdvlp\WebShopAPI\Properties\launchSettings.json .\Magazaki\Apps\WebShopAPI\Properties\launchSettings.json
copy .\localdvlp\BankingRestAPI\appsettings.json .\Nestrix\Apps\BankingRestAPI
copy .\localdvlp\BankingRestAPI\Properties\launchSettings.json .\Nestrix\Apps\BankingRestAPI\Properties\launchSettings.json
REM NIET inchecken met git!
REM vervolgens uitvoeren:
REM .\scripts\StartPeasieAPI.cmd
REM .\scripts\StartNestrixAPI.cmd
REM .\scripts\StartMagazakiAPI.cmd
