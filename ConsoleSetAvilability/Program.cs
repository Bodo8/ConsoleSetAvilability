
using ConsoleSetAvilability;
using ConsoleSetAvilability.Library;

var serviceProvider = DependencyInjection.ConfigureServices();
IMainClass main = (IMainClass)serviceProvider.GetService(typeof(IMainClass));
HttpClient? apiClient = await main.RunUpdatesAvailabilityProducts();
await main.RunUpdateCheckboxesProducts(apiClient);