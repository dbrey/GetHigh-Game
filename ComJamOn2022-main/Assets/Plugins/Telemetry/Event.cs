using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Telemetry
{
    public abstract class Event
    {
        public string Type { get => GetType().Name; }
        public string id_user { get; internal set; }
        public Guid id_session { get; internal set; }
        public int id_game { get; internal set; }
        public long send_time { get; protected set; }

        public Event()
        {
            id_user = "0";
            id_session = Guid.Empty;
            id_game = -1;
            send_time = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }

    // EVENTOS GENERALES =======================================================================
    public class StartSession : Event
    {
        public StartSession()
        {
        }
    }

    public class EndSession : Event
    {
        public EndSession()
        {
        }
    }

    public class StartGame : Event
    {
        public StartGame()
        {
        }
    }

    public class EndGame : Event
    {
        public EndGame()
        {
        }
    }

    public class GameAborted : Event
    {
        public GameAborted()
        {
        }
    }

    // EVENTOS "CABLEADOS" DE GETHIGH =======================================================================

    // OBJETIVO 1
    public class TimeStart : Event
    {
        public int id_quest { get; protected set; }
        public int id_setQuest { get; protected set; }

        public TimeStart(int question, int questionSet)
        {
            id_quest = question;
            id_setQuest = questionSet;
        }

        public TimeStart() : this(0, 0)
        {
        }
    }

    public class TimeReply : Event
    {
        public int id_quest { get; protected set; }
        public int id_setQuest { get; protected set; }
        public bool correct { get; protected set; }

        public TimeReply(int question, int questionSet, bool result)
        {
            id_quest = question;
            id_setQuest = questionSet;
            correct = result;
        }

        public TimeReply() : this(0, 0, false)
        {
        }
    }

    // OBJETIVO 2
    public class GetBlock : Event
    {
        public int id_block { get; protected set; }

        public GetBlock(int block)
        {
            id_block = block;
        }

        public GetBlock() : this(0)
        {
        }
    }

    public class GrabBlock : Event
    {
        int id_block { get; set; }

        public GrabBlock(int block)
        {
            id_block = block;
        }

        public GrabBlock() : this( 0)
        {
        }
    }

    public class ReleaseBlock : Event
    {
        int id_block { get; set; }

        public ReleaseBlock(int block)
        {
            id_block = block;
        }

        public ReleaseBlock() : this(0)
        {
        }
    }
}
