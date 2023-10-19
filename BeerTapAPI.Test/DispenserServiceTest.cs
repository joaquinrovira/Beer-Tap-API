using BeerTapAPI.Dtos;
using BeerTapAPI.Entities;
using BeerTapAPI.Errors;
using BeerTapAPI.Events;
using CSharpFunctionalExtensions;
using Moq;

namespace BeerTapAPI.Test;

internal class DispenserServiceTest : NUnitDI
{
    DispenserService DispenserService;
    Mock<IDispenserRepository> DispenserRepository;

    [SetUp]
    public void Setup()
    {
        DispenserService = Get<DispenserService>();
        DispenserRepository = GetMock<IDispenserRepository>();
    }
    [TearDown]
    public void Teardown()
    {
        DispenserRepository.Verify();
    }

    [Test]
    public void Register_Registers()
    {
        // Setup
        var request = new RegisterDispenserRequest(1);
        DispenserRepository
            .Setup(repo => repo.Register(It.IsAny<Dispenser>()))
            .Returns(UnitResult.Success<Error>());

        // Execute 
        var result = DispenserService.Register(request);

        // Validate
        Assert.That(result.IsSuccess);
    }


    [Test]
    public void SetStatus_AcceptsFirstOpen()
    {
        // Setup
        var request = new SetDispenserStatusRequest(DispenserStatus.open, DateTime.Now);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(UnitResult.Success<Error>());

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsSuccess);
    }


    [Test]
    public void SetStatus_RejectsFirstClose()
    {
        // Setup
        var request = new SetDispenserStatusRequest(DispenserStatus.close, DateTime.Now);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(UnitResult.Success<Error>());

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsFailure);
    }



    [Test]
    public void SetStatus_AcceptsCloseAfterOpen()
    {
        // Setup
        var date = DateTime.Now;
        var request = new SetDispenserStatusRequest(DispenserStatus.close, date);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(UnitResult.Success<Error>());
        DispenserRepository
            .Setup(repo => repo.LastEvent(dispenser.Id))
            .Returns(Result.Success<Maybe<IDispenserEvent>, Error>(Maybe.From(new OpenTapEvent(dispenser.Id, date.AddHours(-1)) as IDispenserEvent)));

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsSuccess);
    }



    [Test]
    public void SetStatus_RejectsDoubleClose()
    {
        // Setup
        var date = DateTime.Now;
        var request = new SetDispenserStatusRequest(DispenserStatus.close, date);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(() => UnitResult.Success<Error>());
        DispenserRepository
            .Setup(repo => repo.LastEvent(dispenser.Id))
            .Returns(Result.Success<Maybe<IDispenserEvent>, Error>(Maybe.From(new CloseTapEvent(dispenser.Id, date.AddHours(-1)) as IDispenserEvent)));

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsFailure);
    }



    [Test]
    public void SetStatus_RejectsDoubleOpen()
    {
        // Setup
        var date = DateTime.Now;
        var request = new SetDispenserStatusRequest(DispenserStatus.open, date);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(() => UnitResult.Success<Error>());
        DispenserRepository
            .Setup(repo => repo.LastEvent(dispenser.Id))
            .Returns(Result.Success<Maybe<IDispenserEvent>, Error>(Maybe.From(new OpenTapEvent(dispenser.Id, date.AddHours(-1)) as IDispenserEvent)));

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsFailure);
    }

    [Test]
    public void SetStatus_AcceptsNullDateTimes()
    {
        // Setup
        var request = new SetDispenserStatusRequest(DispenserStatus.open, null);
        var dispenser = new Dispenser(Guid.NewGuid(), 1);

        DispenserRepository
            .Setup(repo => repo.Get(dispenser.Id))
            .Returns(Result.Success<Dispenser, Error>(dispenser));
        DispenserRepository
            .Setup(repo => repo.PublishEvent(dispenser.Id, It.IsAny<IDispenserEvent>()))
            .Returns(UnitResult.Success<Error>());

        // Execute 
        var result = DispenserService.SetStatus(dispenser.Id, request);

        // Validate
        Assert.That(result.IsSuccess);
    }
}