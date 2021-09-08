using Revolution.Objects.Channel;
using Revolution.Objects.User;

namespace Revolution.Objects.ModelActions
{
    public class ChannelUpdateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string IconId { get; set; }

        public RemoveEnum? Remove { get; set; }
    }
}
