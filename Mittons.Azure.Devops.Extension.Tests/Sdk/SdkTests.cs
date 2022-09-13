// using Microsoft.Extensions.DependencyInjection;
// using Mittons.Azure.Devops.Extension.Sdk;
// using Mittons.Azure.Devops.Extension.Xdm;
// using Moq;

// namespace Mittons.Azure.Devops.Extension.Tests.Sdk;

// public class SdkTests : IDisposable
// {
//     private readonly ServiceCollection _serviceCollection = new();

//     private readonly ServiceProvider _serviceProvider;

//     private readonly Mock<IChannel> _mockChannel = new();

//     public SdkTests()
//     {
//         _serviceCollection.AddSingleton<IChannel>(_mockChannel.Object);
//         _serviceCollection.AddSdk();

//         _serviceProvider = _serviceCollection.BuildServiceProvider();
//     }

//     public void Dispose()
//     {
//         _serviceProvider.Dispose();
//     }

//     [Fact]
//     public async Task InitializeAsync_WhenCalledWithoutArguments_ExpectDefaultsToBeUsed()
//     {
//         await Task.CompletedTask;
//     }
// }
