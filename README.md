
<p align="center">
    <a href="#"><img src="https://i.imgur.com/YKVDbow.jpg" /></a>
    <br />
    <br />
    <a href="https://discord.gg/DcuYTCW"><img alt="Discord" src="https://img.shields.io/discord/581547415316987935.svg?label=Discord&style=for-the-badge"></a>
    <a href="https://github.com/dlive-apis/dlivetv-api-net/blob/master/LICENSE"><img alt="GitHub" src="https://img.shields.io/github/license/dlive-apis/dlivetv-api-net.svg?style=for-the-badge"></a>
    <a href="https://www.nuget.org/packages/dlive-api"><img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/dlivetv-unofficial-api-net.svg?style=for-the-badge"></a>
    <a href="https://www.nuget.org/packages/dlive-api"><img alt="Nuget" src="https://img.shields.io/nuget/dt/dlivetv-api-net.svg?style=for-the-badge"></a>
</p>


> dlivetv-unofficial-api is a wrapping API for the graphql hidden api provided from dlive.tv with a focus on ease of use and on performance. 

## Prerequisites
- Access token is required to use this library. Create an account on [dlive.tv](https://dlive.tv/) then follow our [wiki](https://dlive.timedot.cc/tutorials) tutorial to get your token

## Installation
dlivetv-api is free and easy to install
```bash
Install-Package dlive-api
```
You can install it in .NET CLI or Paket CLI too
```bash
dotnet add package dlive-api
```
```bash
paket add dlive-api
```

## First Usage
```csharp
Console.WriteLine("Press a key to start.");
Console.ReadKey();

var api = new DLive("dlive-0123456789", "Secretkey");

api.Events.OnConnect += (connected, error) =>
{
	if (connected)
		Console.WriteLine("Connected! Ready for live events.");
	else
		Console.WriteLine($"Connection failed...\n{error}");
};

api.Events.OnMessageReceived += async (message, sender) =>
{
	Console.WriteLine($"{sender.DisplayName} wrote: {message.Content}");

	// Delete message after 3 seconds
	Thread.Sleep(3000);
	await message.DeleteMessage();
};

api.Events.OnMessageDeleted += message =>
{
	Console.WriteLine($"Message with id {message.Ids[0]} deleted!");
};

api.ListenEvents(true);

api.Message.SendMessage("Delete me!").Wait();

```
## Documentation
For more information visit our [documentation](https://dlive.timedot.cc/).

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/dlive-apis/dlivetv-api-net/tags). 

## Acknowledgements

- Some graphql queries were taken from [dlive-go-client](https://github.com/Dak425/dlive) written by @Dak425
- [Contributors](https://github.com/dlive-apis/dlivetv-api-net/graphs/contributors)

## See also

- [JavaScript (Node.js)](https://github.com/dlive-apis/dlivetv-api-js) version of dlivetv-unofficial-api
