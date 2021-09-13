using System;

namespace Revolution.Objects.Channel
{
    public interface IChannel
    {
        public Ulid Id { get; }

        public ChannelType ChannelType { get; }
    }
}
