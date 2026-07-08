namespace UsosPotwierdzanieWnioskow.Constants;

public static class Selectors
{
    public const string LoginPageButton = "#actions > a";
    public const string LoginUsername = "#username"; 
    public const string LoginPassword = "#password"; 
    public const string LogInButton = "#fm1 > div.form-button-row > div > div > div > button"; 
    public const string StanFilter = "#tableFilters_";
    public const string ZlozonyFilter = "#tableFilters__4";
    public const string ReadyToAccept = "#tableFilters__0";
    public const string ShowAllButton = "#d_table > table > tbody > tr.headnote.navigation > td > span > a.all_items"; 
    public const string AcceptApplicationButton = "#formularzZmianyStanuAkcje > akcja-zmiany-stanu-dla-koordynatora > input";
    public const string ApplicationsRows = "#d_table > table tbody tr";
    public const string StudentIndex = "td:nth-child(2) > div";
    public const string StudentsDormitoriesRows =
        "#formularzZmianyStanu > span.parametr.prezenterWybierzWierszTabeliZlozony.edycja > table tbody tr";
    public const string SelectDormitoryButton = "#_pwk_przyznany_ds_0";
}