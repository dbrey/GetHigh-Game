public struct SignalOnBecomeVisible 
{
    public SignalOnBecomeVisible(bool visible)
    {
        this.isVisible = visible;
    }
    public bool isVisible; 
}

public struct PlaySoundSignal
{
    public PlaySoundSignal(Sounds sound, float volume = 0.25f)
    {
        this.sound = sound;
        this.volume = volume;
    }

    public Sounds sound;
    public float volume;
}

public struct SignalOnBlockPlaced{};