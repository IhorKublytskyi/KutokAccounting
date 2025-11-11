using KutokAccounting.Services.Vendors;
using KutokAccounting.Services.Vendors.Models;
using NSubstitute;

namespace KutokAccounting.Service.Tests.Vendors;

public class VendorServiceTests
{
	[Fact]
	public async Task CreateVendor()
	{
		//Arrange
		var vendorService = Substitute.For<IVendorService>();

		VendorDto vendorDto = new VendorDto()
		{
			Description = null,
			Name = new string('a', 1000)
		};
		
		//Act
		var result = await vendorService.CreateAsync(vendorDto, default);

		//Assert
		Assert.Null(result);
	}
}