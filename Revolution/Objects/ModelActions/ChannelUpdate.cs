using Newtonsoft.Json;
using Revolution.Objects.Channel;
using System;

namespace Revolution.Objects.ModelActions
{
    public class ChannelUpdateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Ulid? IconId { get; set; }

        public Remove? Remove { get; set; }
    }
}
