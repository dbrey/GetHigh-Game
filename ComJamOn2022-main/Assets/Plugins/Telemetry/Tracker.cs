using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Telemetry.Interfaces;
using Telemetry.PersistanceSystem;
using Telemetry.SerializationSystem;

namespace Telemetry
{
    public sealed class Tracker
    {
        // Miembros =======================================================
        private static Tracker instance;
        private CancellationTokenSource cancellationTokenSource; // Token de cancelación para parar el hilo instantáneamente
        const UInt16 SAVING_FREQUENCY_MS = 5000; //

        private ConcurrentQueue<Event> queue;
        private List<IPersistance> persisters;
        private Thread persistThread;

        // Gestion de eventos
        int gameID;
        string userID;
        Guid sessionID;

        // Constructor privado =============================================
        private Tracker(string userID)
        {
            queue = new ConcurrentQueue<Event>();
            cancellationTokenSource = new CancellationTokenSource();
            persisters = new List<IPersistance>();
            this.userID = userID;
            this.sessionID = Guid.NewGuid();
            this.gameID = -1;
        }

        // Método Init =====================================================
        public static void Init(string userID)
        {
            instance = new Tracker(userID);

            // Por defecto un FilePersistance, pero el usuario puede añadir externamente lo que quiera
            instance.persisters.Add(new FilePersistance(new JSONSerializer(), "./logs.json"));
            instance.TrackEvent(new StartSession());

            // Inicializar hilo
            instance.persistThread = new Thread(() => PersistLoop(instance.cancellationTokenSource.Token));
            instance.persistThread.Start();
        }

        // Método End ======================================================
        public static void End()
        {
            instance.TrackEvent(new EndSession());

            // Solicitar la cancelación
            instance.cancellationTokenSource.Cancel();

            // Esperar a que termine el hilo
            instance.persistThread.Join();

            instance.PersistAllEvents(); // Persistir los eventos restantes
        }

        // Propiedades =====================================================
        public static Tracker Instance => instance; // Sigo sin estar seguro de para que queremos esto

        // Métodos =========================================================
        // Quizas esto podria no ser estático para acceder a través de la instancia
        // pero no sé, también puede ser estático y que el propio método sea el que llame a la instancia, no hay mucha diferencia en verdad
        // si la instancia es null no hará nada y ya. Quizás debería hacer que devuelva un booleano como teniamos antes, pero no estoy seguro, 
        // en la plantilla de guille eso no viene. Ademas el track event no deberia fallar, creo
        public void TrackEvent(Event evnt)
        {
            // Controlar el numero de partidas
            if (evnt is StartGame)
                gameID++;

            evnt.id_session = instance.sessionID;
            evnt.id_user = instance.userID;
            evnt.id_game = instance.gameID;

            queue.Enqueue(evnt);
        }

        public void AddPersistanceSystem(IPersistance persister) => this.persisters.Add(persister);

        private static void PersistLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                int result = WaitHandle.WaitAny(new WaitHandle[] { cancellationToken.WaitHandle }, TimeSpan.FromMilliseconds(SAVING_FREQUENCY_MS));

                if (result != WaitHandle.WaitTimeout)
                    break;

                instance.PersistAllEvents();
            }
        }

        private void PersistAllEvents()
        {
            while (instance.queue.TryDequeue(out var evnt))
            {
                foreach (var persister in instance.persisters)
                {
                    persister.Persist(evnt);
                }
            }
        }
    }
}
