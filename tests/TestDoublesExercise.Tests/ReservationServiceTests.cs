namespace TestDoublesExercise.Tests;

public sealed class ReservationServiceTests
{
    private sealed class Stub(IReadOnlyDictionary<string, int> availableSeatsByEventCode) : ISeatAvailability
    {
        public Stub(string eventCode, int availableSeats)
        : this(new Dictionary<string, int> {[eventCode] = availableSeats} )
        {
            
        }
       public int CallCount { get; private set; }

       public int GetAvailableSeats(string eventCode)
       {
           CallCount++;
           return availableSeatsByEventCode.GetValueOrDefault(eventCode);
       }
       
    }

    private sealed class Repository(bool trySaveResult = true) : IReservationRepository
    {
        public List<Reservation> SavedReservations { get; } = [];

        public bool TrySave(Reservation reservation)
        {
            SavedReservations.Add(reservation);
            return trySaveResult;
        }
    }

    private sealed class Spy : IConfirmationSender
    {
        private readonly List<Reservation> _sentReservations = [];
        public IReadOnlyList<Reservation> SentReservations => _sentReservations;
        public void Send(Reservation reservation) => _sentReservations.Add(reservation);

    }
    
    // Entfernt Skip jeweils erst, wenn ihr den Test implementiert habt.
    // Die Beispiele für Stub, Spy und Moq stehen in TestDoubleExamples.cs.

    [Fact]
    public void Reserve_WithEnoughSeats_ReturnsTrue()
    {
        // Arrange: Stub mit 3 freien Plätzen, Repository mit erfolgreichem Speichern,
        //          eigener Sender-Spy und ReservationService.
        
        // Act:     2 Plätze reservieren.
        
        // Assert:  Ergebnis ist true.
        
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: fehlende Verfügbarkeit prüfen")]
    public void Reserve_WithTooFewSeats_DoesNotSaveOrSend()
    {
        // Arrange: Stub mit 1 freiem Platz und Doubles für Repository und Sender.
        // Act:     2 Plätze reservieren.
        // Assert:  Ergebnis ist false; weder Speichern noch Senden wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: Grenzfall testen")]
    public void Reserve_WithExactlyEnoughSeats_Succeeds()
    {
        // Arrange: Stub mit genau 2 freien Plätzen.
        // Act:     2 Plätze reservieren.
        // Assert:  Ergebnis ist true.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: Platzanzahl 0 testen")]
    public void Reserve_WithZeroSeats_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        // Act:     0 Plätze reservieren.
        // Assert:  ArgumentOutOfRangeException; keine Abhängigkeit wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: negative Platzanzahl testen")]
    public void Reserve_WithNegativeSeats_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        // Act:     -1 Plätze reservieren.
        // Assert:  ArgumentOutOfRangeException; keine Abhängigkeit wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: leeren Veranstaltungscode testen")]
    public void Reserve_WithEmptyEventCode_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        // Act:     Mit leerem Veranstaltungscode reservieren.
        // Assert:  ArgumentException; keine Abhängigkeit wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: E-Mail-Adresse aus Leerzeichen testen")]
    public void Reserve_WithWhitespaceEmail_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Handgeschriebene Doubles oder Moq gezielt auswählen.
        // Act:     Mit einer E-Mail-Adresse aus Leerzeichen reservieren.
        // Assert:  ArgumentException; keine Abhängigkeit wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: fehlgeschlagenes Speichern testen")]
    public void Reserve_WhenSavingFails_DoesNotSendConfirmation()
    {
        // Arrange: Stub mit genügend Plätzen, Repository liefert false, eigener Spy.
        // Act:     Reservierung versuchen.
        // Assert:  Ergebnis ist false und der Spy hat keinen Aufruf aufgezeichnet.
        throw new NotImplementedException();
    }

    [Fact(Skip = "TODO: Moq für fehlenden Speicherversuch verwenden")]
    public void Reserve_WithTooFewSeats_DoesNotCallRepository()
    {
        // Arrange: Stub mit zu wenigen Plätzen und Mock<IReservationRepository>.
        // Act:     Reservierung versuchen.
        // Assert:  Verify(..., Times.Never()) für TrySave.
        throw new NotImplementedException();
    }

    [Fact(Skip = "Fortgeschrittene: ReserveBatch zuerst implementieren")]
    public void ReserveBatch_WhenCombinedSeatsExceedCapacity_DoesNotSaveOrSend()
    {
        // Arrange: Zwei Reservierungen für dieselbe Veranstaltung, die einzeln passen.
        // Act:     Nach Implementierung des Batch-Verfahrens beide zusammen reservieren.
        // Assert:  Ergebnis ist false; kein Batch-Speichern und keine Bestätigung.
        throw new NotImplementedException();
    }
    
}
