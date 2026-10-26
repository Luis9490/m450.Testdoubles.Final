using Moq;

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
        var availability = new Stub("EVT-1", 3);
        var repo = new Repository();
        var sender = new Spy();
        var service = new ReservationService(availability, repo, sender);
        
        // Act:     2 Plätze reservieren.
        var result = service.Reserve("oalgo.martines@empathy.y2k", "EVT-1", 2);
        
        // Assert:  Ergebnis ist true.
        Assert.True(result);
        Assert.Single(repo.SavedReservations);
        Assert.Single(sender.SentReservations);

    }

    [Fact]
    public void Reserve_WithTooFewSeats_DoesNotSaveOrSend()
    {
        // Arrange: Stub mit 1 freiem Platz und Doubles für Repository und Sender.
        var availability = new Stub("EVT-1", 1);
        var repo = new Repository();
        var sender = new Spy();
        var service = new ReservationService(availability, repo, sender);
        // Act:     2 Plätze reservieren.
        var result = service.Reserve("oalgo.martines@empathy.y2k", "EVT-1", 2);
        // Assert:  Ergebnis ist false; weder Speichern noch Senden wurde aufgerufen.
        Assert.False(result);
        Assert.Empty(repo.SavedReservations);
        Assert.Empty(sender.SentReservations);
    }

    [Fact]
    public void Reserve_WithExactlyEnoughSeats_Succeeds()
    {
        // Arrange: Stub mit genau 2 freien Plätzen.
        var availability = new Stub("EVT-1", 2);
        var repo = new Repository();
        var sender = new Spy();
        var service = new ReservationService(availability, repo, sender);
        // Act:     2 Plätze reservieren.
        var result = service.Reserve("oalgo.martines@empathy.y2k", "EVT-1", 2);
        // Assert:  Ergebnis ist true.
        Assert.True(result);
        Assert.Single(repo.SavedReservations);
        Assert.Single(sender.SentReservations);
    }

    [Fact]
    public void Reserve_WithZeroSeats_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        var availability = new Stub("EVT-1", 2);
        var repo = new Repository();
        var sender = new Spy();
        var service = new ReservationService(availability, repo, sender);
        // Act:     0 Plätze reservieren.
        Assert.Throws<ArgumentOutOfRangeException>(() => service.Reserve("oalgo.martines@empathy.y2k", "EVT-1", 0));
        // Assert:  ArgumentOutOfRangeException; keine Abhängigkeit wurde aufgerufen.
        Assert.Equal(0, availability.CallCount);
        Assert.Empty(repo.SavedReservations);
        Assert.Empty(sender.SentReservations);

    }

    [Fact]
    public void Reserve_WithNegativeSeats_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        var availability = new Stub("EVT-1", 2);
        var repo = new Repository();
        var sender = new Spy();
        var service = new ReservationService(availability, repo, sender);
        // Act:     -1 Plätze reservieren.
        Assert.Throws<ArgumentOutOfRangeException>(() => service.Reserve("oalgo.martines@empathy.y2k", "EVT-1", -1));
        // Assert:  ArgumentOutOfRangeException; keine Abhängigkeit wurde aufgerufen.
        Assert.Equal(0, availability.CallCount);
        Assert.Empty(repo.SavedReservations);
        Assert.Empty(sender.SentReservations);
    }

    [Fact(Skip = "TODO: leeren Veranstaltungscode testen")]
    public void Reserve_WithEmptyEventCode_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Doubles, deren Aufrufe sich prüfen lassen.
        // Act:     Mit leerem Veranstaltungscode reservieren.
        // Assert:  ArgumentException; keine Abhängigkeit wurde aufgerufen.
        throw new NotImplementedException();
    }

    [Fact]
    public void Reserve_WithWhitespaceEmail_ThrowsBeforeCallingDependencies()
    {
        // Arrange: Handgeschriebene Doubles oder Moq gezielt auswählen.
        var availability = new Mock<ISeatAvailability>();
        var repo = new Mock<IReservationRepository>();
        var sender = new Mock<IConfirmationSender>();
        var service = new ReservationService(availability.Object, repo.Object, sender.Object);

        
        // Act:     Mit einer E-Mail-Adresse aus Leerzeichen reservieren.
        Assert.Throws<ArgumentException>(() => service.Reserve("   ", "EVT_1", 1));
        
        // Assert:  ArgumentException; keine Abhängigkeit wurde aufgerufen.
        availability.Verify(a => a.GetAvailableSeats(It.IsAny<string>()), Times.Never());
        repo.Verify(r => r.TrySave(It.IsAny<Reservation>()), Times.Never());
        sender.Verify(s => s.Send(It.IsAny<Reservation>()), Times.Never());
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
