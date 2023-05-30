namespace RESTLayer.Model
{
    public static class IBAN
    {
        public static List<Country> Countries { get; private set; } = new()
        {
            new()
            {
                Name = "AFGHANISTAN",
                Currency = "Afghani",
                CurrencyCode = "AFN",
                CurrencyNumber = 971
            },
            new()
            {
                Name = "ÅLAND EILANDEN",
                Currency = "Euro",
                CurrencyCode = "EUR",
                CurrencyNumber = 978
            },
            new()
            {
                Name = "ALBANIE",
                Currency = "Lek",
                CurrencyCode = "ALL",
                CurrencyNumber = 008
            },
            new()
            {
                Name = "ALGERIJE",
                Currency = "Algerijnse Dinar",
                CurrencyCode = "DZD",
                CurrencyNumber = 012
            },
            new()
            {
                Name = "AMERIKAANS SAMOA",
                Currency = "US Dollar",
                CurrencyCode = "USD",
                CurrencyNumber = 840
            },
            new()
            {
                Name = "ANDORRA",
                Currency = "Euro",
                CurrencyCode = "EUR",
                CurrencyNumber = 978
            },
            new()
            {
                Name = "ANGOLA",
                Currency = "Kwanza",
                CurrencyCode = "AOA",
                CurrencyNumber = 973
            },
            new()
            {
                Name = "ANGUILLA",
                Currency = "East Caribbean Dollar",
                CurrencyCode = "XCD",
                CurrencyNumber = 951
            },
            new()
            {
                Name = "ANTARCTICA",
                Currency = "Geen universele munteenheid",
                CurrencyCode = "",
                CurrencyNumber = 0
            },
            new()
            {
                Name = "ANTIGUA EN BARBUDA",
                Currency = "East Caribbean Dollar",
                CurrencyCode = "XCD",
                CurrencyNumber = 951
            },
            new()
            {
                Name = "ARGENTINIE",
                Currency = "Argentijnse Peso",
                CurrencyCode = "ARS",
                CurrencyNumber = 032
            },
            new()
            {
                Name = "ARMENIE",
                Currency = "Armeense Dram",
                CurrencyCode = "AMD",
                CurrencyNumber = 051
            },
            new()
            {
                Name = "ARUBA",
                Currency = "Arubanse Gulden",
                CurrencyCode = "AWG",
                CurrencyNumber = 533
            },
            new()
            {
                Name = "AUSTRALIE",
                Currency = "Australische Dollar",
                CurrencyCode = "AUD",
                CurrencyNumber = 036
            },
            new()
            {
                Name = "OOSTENRIJK",
                Currency = "Euro",
                CurrencyCode = "EUR",
                CurrencyNumber = 978
            },
            new()
            {
                Name = "AZERBEIDJAN",
                Currency = "Azerbeidjaanse Manat",
                CurrencyCode = "AZN",
                CurrencyNumber = 944
            },
            new()
            {
                Name = "BAHAMA'S (DE)",
                Currency = "Bahamiaanse Dollar",
                CurrencyCode = "BSD",
                CurrencyNumber = 044
            },
            new()
            {
                Name = "BAHREIN",
                Currency = "Bahreinse Dinar",
                CurrencyCode = "BHD",
                CurrencyNumber = 048
            },
            new()
            {
                Name = "BANGLADESH",
                Currency = "Taka",
                CurrencyCode = "BDT",
                CurrencyNumber = 050
            },
            new()
            {
                Name = "BARBADOS",
                Currency = "Barbados Dollar",
                CurrencyCode = "BBD",
                CurrencyNumber = 052
            },
            new()
            {
                Name = "WIT RUSLAND",
                Currency = "Wit Russische Roebel",
                CurrencyCode = "BYR",
                CurrencyNumber = 974
            },
            new()
            {
                Name = "BELGIE",
                Currency = "Euro",
                CurrencyCode = "EUR",
                CurrencyNumber = 978
            },
            new()
            {
                Name = "BELIZE",
                Currency = "Belize Dollar",
                CurrencyCode = "BZD",
                CurrencyNumber = 084
            },
            new()
            {
                Name = "BENIN",
                Currency = "CFA Franc BCEAO",
                CurrencyCode = "XOF",
                CurrencyNumber = 952
            },
            new()
            {
                Name = "BERMUDA",
                Currency = "Bermudiaanse Dollar",
                CurrencyCode = "BMD",
                CurrencyNumber = 060
            },
            new()
            {
                Name = "BHUTAN",
                Currency = "Ngultrum",
                CurrencyCode = "BTN",
                CurrencyNumber = 064
            },
            new()
            {
                Name = "BHUTAN",
                Currency = "Indiasche Roepie",
                CurrencyCode = "INR",
                CurrencyNumber = 356
            },
            new()
            {
                Name = "BOLIVIA (PLURINATIONALE STAAT )",
                Currency = "Boliviano",
                CurrencyCode = "BOB",
                CurrencyNumber = 068
            },
            new()
            {
                Name = "BOLIVIA (PLURINATIONALE STAAT)",
                Currency = "Mvdol",
                CurrencyCode = "BOV",
                CurrencyNumber = 984
            },
            new()
            {
                Name = "BONAIRE, SINT EUSTATIUS EN SABA",
                Currency = "US Dollar",
                CurrencyCode = "USD",
                CurrencyNumber = 840
            },
            new()
            {
                Name = "BOSNIE EN HERZEGOVINA",
                Currency = "Convertible Mark",
                CurrencyCode = "BAM",
                CurrencyNumber = 977
            },
            new()
            {
                Name = "BOTSWANA",
                Currency = "Pula",
                CurrencyCode = "BWP",
                CurrencyNumber = 072
            },
            new()
            {
                Name = "BOUVET EILAND",
                Currency = "Norweegse Kroon",
                CurrencyCode = "NOK",
                CurrencyNumber = 578
            },
            new()
            {
                Name = "BRAZILIE",
                Currency = "Braziliaanse Real",
                CurrencyCode = "BRL",
                CurrencyNumber = 986
            },
            new()
            {
                Name = "BRITSE INDISCHE OCEAANTERRITORIUM (HET)",
                Currency = "US Dollar",
                CurrencyCode = "USD",
                CurrencyNumber = 840
            },
            new()
            {
                Name = "BRUNEI",
                Currency = "Brunei Dollar",
                CurrencyCode = "BND",
                CurrencyNumber = 096
            },
            new()
            {
                Name = "BULGARIJE",
                Currency = "Bulgaarse Lev",
                CurrencyCode = "BGN",
                CurrencyNumber = 975
            },
            new()
            {
                Name = "BURKINA FASO",
                Currency = "CFA Franc BCEAO",
                CurrencyCode = "XOF",
                CurrencyNumber = 952
            }
        };
        /*
        new() {
          <td>BURUNDI</td>
          <td>Burundi Franc</td>
          <td>BIF</td>
          <td>108</td>
        },
        new() {
          <td>KAAPVERDIE</td>
          <td>Kaapverdiaanse Escudo</td>
          <td>CVE</td>
          <td>132</td>
        },
        new() {
          <td>CAMBODJA</td>
          <td>Riel</td>
          <td>KHR</td>
          <td>116</td>
        },
        new() {
          <td>CAMEROEN</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>CANADA</td>
          <td>Canadese Dollar</td>
          <td>CAD</td>
          <td>124</td>
        },
        new() {
          <td>KAAIMAN EILANDEN (DE)</td>
          <td>Kaaiman Eilanden Dollar</td>
          <td>KYD</td>
          <td>136</td>
        },
        new() {
          <td>CENTRAAL AFRIKAANSE REPUBLIEK    (DE)</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>TSJAAD</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>CHILI</td>
          <td>Unidad de Fomento</td>
          <td>CLF</td>
          <td>990</td>
        },
        new() {
          <td>CHILI</td>
          <td>Chileense Peso</td>
          <td>CLP</td>
          <td>152</td>
        },
        new() {
          <td>CHINA</td>
          <td>Yuan Renminbi</td>
          <td>CNY</td>
          <td>156</td>
        },
        new() {
          <td>CHRISTMAS EIlAND</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>COCOS (KEELING) EILANDEN (DE)</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>COLOMBIA</td>
          <td>Colombische Peso</td>
          <td>COP</td>
          <td>170</td>
        },
        new() {
          <td>COLOMBIA</td>
          <td>Unidad de Valor Real</td>
          <td>COU</td>
          <td>970</td>
        },
        new() {
          <td>COMOREN (DE)</td>
          <td>Comoro Franc</td>
          <td>KMF</td>
          <td>174</td>
        },
        new() {
          <td>CONGO (DE DEMOCRATISCHE    REPUBLIEK VAN DE)</td>
          <td>Congolese Franc</td>
          <td>CDF</td>
          <td>976</td>
        },
        new() {
          <td>CONGO (DE)</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>COOK EILANDEN (DE)</td>
          <td>Nieuw Zeelandse Dollar</td>
          <td>NZD</td>
          <td>554</td>
        },
        new() {
          <td>COSTA RICA</td>
          <td>Costa Ricaanse Colon</td>
          <td>CRC</td>
          <td>188</td>
        },
        new() {
          <td>IVOORKUST</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>CROATIE</td>
          <td>Kuna</td>
          <td>HRK</td>
          <td>191</td>
        },
        new() {
          <td>CUBA</td>
          <td>Peso Convertible</td>
          <td>CUC</td>
          <td>931</td>
        },
        new() {
          <td>CUBA</td>
          <td>Cubaanse Peso</td>
          <td>CUP</td>
          <td>192</td>
        },
        new() {
          <td>CURAÇAO</td>
          <td>Nederlands Antilliaanse Gulden</td>
          <td>ANG</td>
          <td>532</td>
        },
        new() {
          <td>CYPRUS</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>TJECHISCHE REPUBLIEK (DE)</td>
          <td>Tjechische Kroon</td>
          <td>CZK</td>
          <td>203</td>
        },
        new() {
          <td>DENEMARKEN</td>
          <td>Deense Kroon</td>
          <td>DKK</td>
          <td>208</td>
        },
        new() {
          <td>DJIBOUTI</td>
          <td>Djibouti Franc</td>
          <td>DJF</td>
          <td>262</td>
        },
        new() {
          <td>DOMINICA</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>DOMINICAANSE REPUBLIEK (DE)</td>
          <td>Dominicaanse Peso</td>
          <td>DOP</td>
          <td>214</td>
        },
        new() {
          <td>ECUADOR</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>EGYPTE</td>
          <td>Egyptische Pond</td>
          <td>EGP</td>
          <td>818</td>
        },
        new() {
          <td>EL SALVADOR</td>
          <td>El Salvador Colon</td>
          <td>SVC</td>
          <td>222</td>
        },
        new() {
          <td>EL SALVADOR</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>EQUATORIAAL GUINEA</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>ERITREA</td>
          <td>Nakfa</td>
          <td>ERN</td>
          <td>232</td>
        },
        new() {
          <td>ESTLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>ETHIOPIE</td>
          <td>Ethiopische Birr</td>
          <td>ETB</td>
          <td>230</td>
        },
        new() {
          <td>EUROPESE UNIE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>FALKLAND EILANDEN (DE)[MALVINAS]</td>
          <td>Falkland Eilanden Pond</td>
          <td>FKP</td>
          <td>238</td>
        },
        new() {
          <td>FAROE EILANDEN (DE)</td>
          <td>Deense Kroon</td>
          <td>DKK</td>
          <td>208</td>
        },
        new() {
          <td>FIJI</td>
          <td>Fiji Dollar</td>
          <td>FJD</td>
          <td>242</td>
        },
        new() {
          <td>FINLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>FRANKRIJK</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>FRANS GUYANA</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>FRENCH POLYNESIE</td>
          <td>CFP Franc</td>
          <td>XPF</td>
          <td>953</td>
        },
        new() {
          <td>FRANSE ZUIDELIJKE TERRITORIA    (DE)</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>GABON</td>
          <td>CFA Franc BEAC</td>
          <td>XAF</td>
          <td>950</td>
        },
        new() {
          <td>GAMBIA (DE)</td>
          <td>Dalasi</td>
          <td>GMD</td>
          <td>270</td>
        },
        new() {
          <td>GEORGIE</td>
          <td>Lari</td>
          <td>GEL</td>
          <td>981</td>
        },
        new() {
          <td>DUITSLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>GHANA</td>
          <td>Ghana Cedi</td>
          <td>GHS</td>
          <td>936</td>
        },
        new() {
          <td>GIBRALTAR</td>
          <td>Gibraltar Pond</td>
          <td>GIP</td>
          <td>292</td>
        },
        new() {
          <td>GRIEKENLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>GROENLAND</td>
          <td>Deense Kroon</td>
          <td>DKK</td>
          <td>208</td>
        },
        new() {
          <td>GRENADA</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>GUADELOUPE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>GUAM</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>GUATEMALA</td>
          <td>Quetzal</td>
          <td>GTQ</td>
          <td>320</td>
        },
        new() {
          <td>GUERNSEY</td>
          <td>Pond Sterling</td>
          <td>GBP</td>
          <td>826</td>
        },
        new() {
          <td>GUINEE</td>
          <td>Guinee Franc</td>
          <td>GNF</td>
          <td>324</td>
        },
        new() {
          <td>GUINEE-BISSAU</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>GUYANA</td>
          <td>Guyana Dollar</td>
          <td>GYD</td>
          <td>328</td>
        },
        new() {
          <td>HAITI</td>
          <td>Gourde</td>
          <td>HTG</td>
          <td>332</td>
        },
        new() {
          <td>HAITI</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>HEARD ISLAND EN McDONALD    EILANDEN</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>VATICAANSTAD (DE)</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>HONDURAS</td>
          <td>Lempira</td>
          <td>HNL</td>
          <td>340</td>
        },
        new() {
          <td>HONG KONG</td>
          <td>Hong Kong Dollar</td>
          <td>HKD</td>
          <td>344</td>
        },
        new() {
          <td>HONGARIJE</td>
          <td>Forint</td>
          <td>HUF</td>
          <td>348</td>
        },
        new() {
          <td>IJSLAND</td>
          <td>IJslandse Kroon</td>
          <td>ISK</td>
          <td>352</td>
        },
        new() {
          <td>INDIA</td>
          <td>Indiase Roepie</td>
          <td>INR</td>
          <td>356</td>
        },
        new() {
          <td>INDONESIE</td>
          <td>Roepia</td>
          <td>IDR</td>
          <td>360</td>
        },
        new() {
          <td>INTERNATIONAAL MONETAIR FONDS    (IMF)&nbsp;</td>
          <td>SDR(Speciale trekkingsrechten)</td>
          <td>XDR</td>
          <td>960</td>
        },
        new() {
          <td>IRAN(ISLAMITISCHE REUPLIEK VAN)</td>
          <td>Iraanse Rial</td>
          <td>IRR</td>
          <td>364</td>
        },
        new() {
          <td>IRAK</td>
          <td>Iraakse Dinar</td>
          <td>IQD</td>
          <td>368</td>
        },
        new() {
          <td>IERLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>EILAND MAN</td>
          <td>Pound Sterling</td>
          <td>GBP</td>
          <td>826</td>
        },
        new() {
          <td>ISRAEL</td>
          <td>Nieuwe Israelische shekel</td>
          <td>ILS</td>
          <td>376</td>
        },
        new() {
          <td>ITALIE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>JAMAICA</td>
          <td>Jamaicaanse Dollar</td>
          <td>JMD</td>
          <td>388</td>
        },
        new() {
          <td>JAPAN</td>
          <td>Yen</td>
          <td>JPY</td>
          <td>392</td>
        },
        new() {
          <td>JERSEY</td>
          <td>Pound Sterling</td>
          <td>GBP</td>
          <td>826</td>
        },
        new() {
          <td>JORDANIE</td>
          <td>Jordaanse Dinar</td>
          <td>JOD</td>
          <td>400</td>
        },
        new() {
          <td>KAZAKHSTAN</td>
          <td>Tenge</td>
          <td>KZT</td>
          <td>398</td>
        },
        new() {
          <td>KENIA</td>
          <td>Keniaanse Shilling</td>
          <td>KES</td>
          <td>404</td>
        },
        new() {
          <td>KIRIBATI</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>KOREA (DE DEMOCRATISCHE    VOLKSREPUBLIEK VAN)</td>
          <td>Noordkoreaanse Won</td>
          <td>KPW</td>
          <td>408</td>
        },
        new() {
          <td>KOREA (DE REPUBLIEK VAN)</td>
          <td>Won</td>
          <td>KRW</td>
          <td>410</td>
        },
        new() {
          <td>KOEWEIT</td>
          <td>Kuweiti Dinar</td>
          <td>KWD</td>
          <td>414</td>
        },
        new() {
          <td>KYRGYZSTAN</td>
          <td>Som</td>
          <td>KGS</td>
          <td>417</td>
        },
        new() {
          <td>LAOS DEMOCRATISCHE VOLKSREPUBLIEK (DE)</td>
          <td>Kip</td>
          <td>LAK</td>
          <td>418</td>
        },
        new() {
          <td>LETLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>LIBANON</td>
          <td>Libanese Pond</td>
          <td>LBP</td>
          <td>422</td>
        },
        new() {
          <td>LESOTHO</td>
          <td>Loti</td>
          <td>LSL</td>
          <td>426</td>
        },
        new() {
          <td>LESOTHO</td>
          <td>Rand</td>
          <td>ZAR</td>
          <td>710</td>
        },
        new() {
          <td>LIBERIA</td>
          <td>Liberiaanse Dollar</td>
          <td>LRD</td>
          <td>430</td>
        },
        new() {
          <td>LIBIE</td>
          <td>Libische Dinar</td>
          <td>LYD</td>
          <td>434</td>
        },
        new() {
          <td>LIECHTENSTEIN</td>
          <td>Zwitsere Franc</td>
          <td>CHF</td>
          <td>756</td>
        },
        new() {
          <td>LITOUWEN</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>LUXEMBURG</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>MACAO</td>
          <td>Pataca</td>
          <td>MOP</td>
          <td>446</td>
        },
        new() {
          <td>MACEDONIE (DE VOORMALIGE JOEGOSLAVISCHE REUPLIEK VAN)</td>
          <td>Denar</td>
          <td>MKD</td>
          <td>807</td>
        },
        new() {
          <td>MADAGASCAR</td>
          <td>Malagasy Ariary</td>
          <td>MGA</td>
          <td>969</td>
        },
        new() {
          <td>MALAWI</td>
          <td>Kwacha</td>
          <td>MWK</td>
          <td>454</td>
        },
        new() {
          <td>MALEISIE</td>
          <td>Maleisische Ringgit</td>
          <td>MYR</td>
          <td>458</td>
        },
        new() {
          <td>MALEDIVEN</td>
          <td>Rufiyaa</td>
          <td>MVR</td>
          <td>462</td>
        },
        new() {
          <td>MALI</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>MALTA</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>MARSHALL EILANDEN (DE)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>MARTINIQUE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>MAURITANIE</td>
          <td>Ouguiya</td>
          <td>MRO</td>
          <td>478</td>
        },
        new() {
          <td>MAURITIUS</td>
          <td>Mauritius Roepie</td>
          <td>MUR</td>
          <td>480</td>
        },
        new() {
          <td>MAYOTTE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>LIDSTATEN VAN DE AFRICAN DEVELOPMENT BANK GROUP</td>
          <td>ADB rekeneenheid</td>
          <td>XUA</td>
          <td>965</td>
        },
        new() {
          <td>MEXICO</td>
          <td>Mexicaanse Peso</td>
          <td>MXN</td>
          <td>484</td>
        },
        new() {
          <td>MEXICO</td>
          <td>Mexican Unidad de Inversion (UDI)</td>
          <td>MXV</td>
          <td>979</td>
        },
        new() {
          <td>MICRONESIE (FEDERALE STATEN     VAN)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>MOLDAVIE (DE REPUBLIEK VAN)</td>
          <td>Moldovaarse Leu</td>
          <td>MDL</td>
          <td>498</td>
        },
        new() {
          <td>MONACO</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>MONGOLIE</td>
          <td>Tugrik</td>
          <td>MNT</td>
          <td>496</td>
        },
        new() {
          <td>MONTENEGRO</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>MONTSERRAT</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>MAROKKO</td>
          <td>Marokkaanse Dirham</td>
          <td>MAD</td>
          <td>504</td>
        },
        new() {
          <td>MOZAMBIQUE</td>
          <td>Mozambique Metical</td>
          <td>MZN</td>
          <td>943</td>
        },
        new() {
          <td>MYANMAR</td>
          <td>Kyat</td>
          <td>MMK</td>
          <td>104</td>
        },
        new() {
          <td>NAMIBIE</td>
          <td>Namibie Dollar</td>
          <td>NAD</td>
          <td>516</td>
        },
        new() {
          <td>NAMIBIE</td>
          <td>Rand</td>
          <td>ZAR</td>
          <td>710</td>
        },
        new() {
          <td>NAURU</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>NEPAL</td>
          <td>Nepalese Roepie</td>
          <td>NPR</td>
          <td>524</td>
        },
        new() {
          <td>NEDERLAND</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>NIEW CALEDONIE</td>
          <td>CFP Franc</td>
          <td>XPF</td>
          <td>953</td>
        },
        new() {
          <td>NIEUW ZEELAND</td>
          <td>Nieuw Zeeland Dollar</td>
          <td>NZD</td>
          <td>554</td>
        },
        new() {
          <td>NICARAGUA</td>
          <td>Cordoba Oro</td>
          <td>NIO</td>
          <td>558</td>
        },
        new() {
          <td>NIGER (DE)</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>NIGERIA</td>
          <td>Naira</td>
          <td>NGN</td>
          <td>566</td>
        },
        new() {
          <td>NIUE</td>
          <td>Nieuwe Zeeland Dollar</td>
          <td>NZD</td>
          <td>554</td>
        },
        new() {
          <td>NORFOLK EILAND</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>NOOORDELIJKE MARIANA EILANDEN    (DE)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>NORWEGEN</td>
          <td>Noorse Kroon</td>
          <td>NOK</td>
          <td>578</td>
        },
        new() {
          <td>OMAN</td>
          <td>Rial Omani</td>
          <td>OMR</td>
          <td>512</td>
        },
        new() {
          <td>PAKISTAN</td>
          <td>Pakistan Roepie</td>
          <td>PKR</td>
          <td>586</td>
        },
        new() {
          <td>PALAU</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>PALESTINA, DE STAAT</td>
          <td>Geen universele munteenheid</td>
          <td></td>
          <td></td>
        },
        new() {
          <td>PANAMA</td>
          <td>Balboa</td>
          <td>PAB</td>
          <td>590</td>
        },
        new() {
          <td>PANAMA</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>PAPUA NIEUW GUINEA</td>
          <td>Kina</td>
          <td>PGK</td>
          <td>598</td>
        },
        new() {
          <td>PARAGUAY</td>
          <td>Guarani</td>
          <td>PYG</td>
          <td>600</td>
        },
        new() {
          <td>PERU</td>
          <td>Nuevo Sol</td>
          <td>PEN</td>
          <td>604</td>
        },
        new() {
          <td>FIlIPPIJNEN (DE)</td>
          <td>Filippijnse Peso</td>
          <td>PHP</td>
          <td>608</td>
        },
        new() {
          <td>PITCAIRN</td>
          <td>Nieuw Zeelandse Dollar</td>
          <td>NZD</td>
          <td>554</td>
        },
        new() {
          <td>POLEN</td>
          <td>Zloty</td>
          <td>PLN</td>
          <td>985</td>
        },
        new() {
          <td>PORTUGAL</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>PUERTO RICO</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>QATAR</td>
          <td>Qatari Rial</td>
          <td>QAR</td>
          <td>634</td>
        },
        new() {
          <td>RÉUNION</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>ROEMENIE</td>
          <td>Roemeense Leu</td>
          <td>RON</td>
          <td>946</td>
        },
        new() {
          <td>RUSSISCHE FEDERATIE (DE)</td>
          <td>Russische Roebel</td>
          <td>RUB</td>
          <td>643</td>
        },
        new() {
          <td>RWANDA</td>
          <td>Rwandese Franc</td>
          <td>RWF</td>
          <td>646</td>
        },
        new() {
          <td>SINT BARTHÉLEMY</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SINT HELENA, ASCENSION EN    TRISTAN DA CUNHA</td>
          <td>Saint Helena Pond</td>
          <td>SHP</td>
          <td>654</td>
        },
        new() {
          <td>SAINT KITTS EN NEVIS</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>SAINT LUCIA</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>SINT MAARTEN (FRANSE GEDEELTE)</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SAINT PIERRE EN MIQUELON</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SINT VINCENT EN DE     GRENADINES</td>
          <td>East Caribbean Dollar</td>
          <td>XCD</td>
          <td>951</td>
        },
        new() {
          <td>SAMOA</td>
          <td>Tala</td>
          <td>WST</td>
          <td>882</td>
        },
        new() {
          <td>SAN MARINO</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SAO TOME EN PRINCIPE</td>
          <td>Dobra</td>
          <td>STD</td>
          <td>678</td>
        },
        new() {
          <td>SAUDI ARABIE</td>
          <td>Saudi Riyal</td>
          <td>SAR</td>
          <td>682</td>
        },
        new() {
          <td>SENEGAL</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>SERVIE</td>
          <td>Servische Dinar</td>
          <td>RSD</td>
          <td>941</td>
        },
        new() {
          <td>SEYCHELLEN</td>
          <td>Seychellen Roepie</td>
          <td>SCR</td>
          <td>690</td>
        },
        new() {
          <td>SIERRA LEONE</td>
          <td>Leone</td>
          <td>SLL</td>
          <td>694</td>
        },
        new() {
          <td>SINGAPORE</td>
          <td>Singapore Dollar</td>
          <td>SGD</td>
          <td>702</td>
        },
        new() {
          <td>SINT MAARTEN (NEDERLANDSE GEDEELTE)</td>
          <td>Nederlands Antilliaanse Gulden</td>
          <td>ANG</td>
          <td>532</td>
        },
        new() {
          <td>SISTEMA UNITARIO DE COMPENSACION REGIONAL DE PAGOS &quot; SUCRE&quot;</td>
          <td>Sucre</td>
          <td>XSU</td>
          <td>994</td>
        },
        new() {
          <td>SLOWAKIJE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SLOVENIE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SOLOMON EILANDEN</td>
          <td>Solomon Eilanden Dollar</td>
          <td>SBD</td>
          <td>090</td>
        },
        new() {
          <td>SOMALIE</td>
          <td>Somalische Shilling</td>
          <td>SOS</td>
          <td>706</td>
        },
        new() {
          <td>ZUID AFRICA</td>
          <td>Rand</td>
          <td>ZAR</td>
          <td>710</td>
        },
        new() {
          <td>SOUTH GEORGIA EN DE SOUTH SANDWICH ISLANDS</td>
          <td>Geen universele munteenheid</td>
          <td></td>
          <td></td>
        },
        new() {
          <td>ZUID SOEDAN</td>
          <td>Zuid Soedanese Pond</td>
          <td>SSP</td>
          <td>728</td>
        },
        new() {
          <td>SPANJE</td>
          <td>Euro</td>
          <td>EUR</td>
          <td>978</td>
        },
        new() {
          <td>SRI LANKA</td>
          <td>Sri Lankese Roepie</td>
          <td>LKR</td>
          <td>144</td>
        },
        new() {
          <td>SOEDAN (DE)</td>
          <td>Soedanese Pond</td>
          <td>SDG</td>
          <td>938</td>
        },
        new() {
          <td>SURINAME</td>
          <td>Surinaamse Dollar</td>
          <td>SRD</td>
          <td>968</td>
        },
        new() {
          <td>SPITSBERGEN EN JAN MAYEN</td>
          <td>Noorse Kroong</td>
          <td>NOK</td>
          <td>578</td>
        },
        new() {
          <td>SWAZILAND</td>
          <td>Lilangeni</td>
          <td>SZL</td>
          <td>748</td>
        },
        new() {
          <td>ZWEDEN</td>
          <td>Zweedse Kroon</td>
          <td>SEK</td>
          <td>752</td>
        },
        new() {
          <td>ZWITSERLAND</td>
          <td>WIR Euro</td>
          <td>CHE</td>
          <td>947</td>
        },
        new() {
          <td>ZWITSERLAND</td>
          <td>Zwitserse Franc</td>
          <td>CHF</td>
          <td>756</td>
        },
        new() {
          <td>ZWITSERLAND</td>
          <td>WIR Franc</td>
          <td>CHW</td>
          <td>948</td>
        },
        new() {
          <td>SYRISCHE ARABISCHE REPUBLIEK</td>
          <td>Syrische Pond</td>
          <td>SYP</td>
          <td>760</td>
        },
        new() {
          <td>TAIWAN (PROVINCIE VAN CHINA)</td>
          <td>Nieuw Taiwanese Dollar</td>
          <td>TWD</td>
          <td>901</td>
        },
        new() {
          <td>TADJIKISTAN</td>
          <td>Somoni</td>
          <td>TJS</td>
          <td>972</td>
        },
        new() {
          <td>TANZANIA, VERENIGDE REPUBLIEK VAN</td>
          <td>Tanzaniaanse Shilling</td>
          <td>TZS</td>
          <td>834</td>
        },
        new() {
          <td>THAILAND</td>
          <td>Baht</td>
          <td>THB</td>
          <td>764</td>
        },
        new() {
          <td>TIMOR-LESTE</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>TOGO</td>
          <td>CFA Franc BCEAO</td>
          <td>XOF</td>
          <td>952</td>
        },
        new() {
          <td>TOKELAU</td>
          <td>Nieuw Zeelandse Dollar</td>
          <td>NZD</td>
          <td>554</td>
        },
        new() {
          <td>TONGA</td>
          <td>Pa&rsquo; anga</td>
          <td>TOP</td>
          <td>776</td>
        },
        new() {
          <td>TRINIDAD EN TOBAGO</td>
          <td>Trinidad en Tobago Dollar</td>
          <td>TTD</td>
          <td>780</td>
        },
        new() {
          <td>TUNISIE</td>
          <td>Tunisische Dinar</td>
          <td>TND</td>
          <td>788</td>
        },
        new() {
          <td>TURKIJE</td>
          <td>Turkse Lira</td>
          <td>TRY</td>
          <td>949</td>
        },
        new() {
          <td>TURKMENISTAN</td>
          <td>Turkmeense Nieuwe Manat</td>
          <td>TMT</td>
          <td>934</td>
        },
        new() {
          <td>TURKS EN CAICOS EILANDEN    (DE)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>TUVALU</td>
          <td>Australische Dollar</td>
          <td>AUD</td>
          <td>036</td>
        },
        new() {
          <td>OEGANDA</td>
          <td>Oegandese Shilling</td>
          <td>UGX</td>
          <td>800</td>
        },
        new() {
          <td>OEKRAINE</td>
          <td>Hryvnia</td>
          <td>UAH</td>
          <td>980</td>
        },
        new() {
          <td>VERENIGDE ARABISCHE EMIRATEN (DE)</td>
          <td>UAE Dirham</td>
          <td>AED</td>
          <td>784</td>
        },
        new() {
          <td>VERENIGD KONINKRIJK VAN GROOT BRITANNIE EN NOORD IERLAND (DE)</td>
          <td>Pound Sterling</td>
          <td>GBP</td>
          <td>826</td>
        },
        new() {
          <td>VERENIGDE STATEN AFGELEGEN EILANDEN</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>VERENIGDE STATEN VAN AMERIKA    (DE)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>VERENIGDE STATEN VAN AMERIKA    (DE)</td>
          <td>US Dollar (Next day)</td>
          <td>USN</td>
          <td>997</td>
        },
        new() {
          <td>URUGUAY</td>
          <td>Uruguay Peso en Unidades Indexadas (URUIURUI)</td>
          <td>UYI</td>
          <td>940</td>
        },
        new() {
          <td>URUGUAY</td>
          <td>Peso Uruguayo</td>
          <td>UYU</td>
          <td>858</td>
        },
        new() {
          <td>OEZBEKISTAN</td>
          <td>Oezbekistan Sum</td>
          <td>UZS</td>
          <td>860</td>
        },
        new() {
          <td>VANUATU</td>
          <td>Vatu</td>
          <td>VUV</td>
          <td>548</td>
        },
        new() {
          <td>VENEZUELA (BOLIVAARSE REPUBLIEK VAN)</td>
          <td>Bolivar</td>
          <td>VEF</td>
          <td>937</td>
        },
        new() {
          <td>VIETNAM</td>
          <td>Dong</td>
          <td>VND</td>
          <td>704</td>
        },
        new() {
          <td>MAAGDENEILANDEN(BRITSE)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>MAAGDENEILANDEN (V.S.)</td>
          <td>US Dollar</td>
          <td>USD</td>
          <td>840</td>
        },
        new() {
          <td>WALLIS EN FUTUNA</td>
          <td>CFP Franc</td>
          <td>XPF</td>
          <td>953</td>
        },
        new() {
          <td>WESTELIJKE SAHARA</td>
          <td>Marokkaanse Dirham</td>
          <td>MAD</td>
          <td>504</td>
        },
        new() {
          <td>YEMEN</td>
          <td>Yemeni Rial</td>
          <td>YER</td>
          <td>886</td>
        },
        new() {
          <td>ZAMBIA</td>
          <td>Zambiase Kwacha</td>
          <td>ZMW</td>
          <td>967</td>
        },
        new() {
          <td>ZIMBABWE</td>
          <td>Zimbabwese Dollar</td>
          <td>ZWL</td>
          <td>932</td>
        },
        </tbody>
      </table>
        */
    }
}