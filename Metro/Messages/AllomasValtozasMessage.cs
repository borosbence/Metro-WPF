using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Metro.Messages
{
    public class AllomasMessage
    {
        public AllomasMessage(bool indulo, string allomasNev)
        {
            Indulo = indulo;
            AllomasNev = allomasNev;
        }

        public bool Indulo { get; set; }
        public string AllomasNev { get; set; } = string.Empty;
    }

    public class AllomasValtozasMessage : ValueChangedMessage<AllomasMessage>
    {
        public AllomasValtozasMessage(AllomasMessage value) : base(value)
        {
        }
    }
}
