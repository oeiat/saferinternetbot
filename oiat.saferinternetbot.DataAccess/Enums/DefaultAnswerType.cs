using mbit.common.Attributes;

namespace oiat.saferinternetbot.DataAccess.Enums
{
    public enum DefaultAnswerType
    {
        [McEnumDisplay(Name="Ungültige Nachricht")]
        InvalidMessage = 0,
        [McEnumDisplay(Name = "Keine Antworten verfügbar")]
        NoAnswerAvailable = 1,
        [McEnumDisplay(Name = "Zeitlich limitierte Zusatz-Nachrichten")]
        TimeRestrictedMessage = 2
    }
}
