using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using WestoverLaneReserve.Pages;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WestoverLaneReserve.Tests.Pages
{
    public class MyReservationsModelTests
    {
        private readonly Mock<UserManager<CustomerApplicationUser>> _userManagerMock;
        private readonly ApplicationDbContext _context;
        private readonly MyReservationsModel _pageModel;

        public MyReservationsModelTests()
        {
            var userStoreMock = new Mock<IUserStore<CustomerApplicationUser>>();
            _userManagerMock = new Mock<UserManager<CustomerApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            _pageModel = new MyReservationsModel(_context, _userManagerMock.Object);

            // Mock HttpContext to add a ClaimsPrincipal user
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "testUserId")
                }, "mock"))
            };

            var modelMetadataProvider = new EmptyModelMetadataProvider();

            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor(), new ModelStateDictionary());

            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var viewData = new ViewDataDictionary(modelMetadataProvider, new ModelStateDictionary());

            _pageModel.PageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
        }

        [Fact]
        public async Task OnGetAsync_SetsReservations_ForCurrentUser()
        {
            // Arrange
            var user = new CustomerApplicationUser { Id = "testUserId" };
            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var reservations = new List<LaneReservation>
            {
                new LaneReservation { CustomerId = "testUserId", Date = DateTime.Today.ToString("yyyy-MM-dd"), Time = "09:00" },
                new LaneReservation { CustomerId = "testUserId", Date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"), Time = "10:00" }
            };
            await _context.LaneReservations.AddRangeAsync(reservations);
            await _context.SaveChangesAsync();

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.Equal(2, _pageModel.Reservations.Count);
        }

        [Fact]
        public async Task OnPostCancelAsync_RemovesReservationAndRedirects()
        {
            // Arrange
            var reservation = new LaneReservation { CustomerId = "testUserId", Date = DateTime.Today.ToString("yyyy-MM-dd"), Time = "09:00" };
            await _context.LaneReservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            // Act
            var result = await _pageModel.OnPostCancelAsync(reservation.ReservationId);

            // Assert
            _context.Entry(reservation).State = EntityState.Detached; // Detach the entity for a clean check
            var removedReservation = await _context.LaneReservations.FindAsync(reservation.ReservationId);
            Assert.Null(removedReservation); // Assert that the reservation was removed
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/MyReservations", redirectResult.PageName);
        }

        // [Fact]
        // public async Task OnGetAsync_FiltersReservationsByDate()
        // {
        //     // Arrange
        //     var user = new CustomerApplicationUser { Id = "testUserId" };
        //     _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        //     var today = DateTime.Today;
        //     var reservations = new List<LaneReservation>
        //     {
        //         new LaneReservation { CustomerId = "testUserId", Date = today.AddDays(-1).ToString("yyyy-MM-dd"), Time = "09:00" }, // Past reservation
        //         new LaneReservation { CustomerId = "testUserId", Date = today.ToString("yyyy-MM-dd"), Time = "10:00" }, // Today's reservation
        //         new LaneReservation { CustomerId = "testUserId", Date = today.AddDays(1).ToString("yyyy-MM-dd"), Time = "11:00" }, // Future reservation
        //         new LaneReservation { CustomerId = "anotherUserId", Date = today.AddDays(1).ToString("yyyy-MM-dd"), Time = "11:00" } // Another user's reservation
        //     };
        //     await _context.LaneReservations.AddRangeAsync(reservations);
        //     await _context.SaveChangesAsync();

        //     // Act
        //     await _pageModel.OnGetAsync();

        //     // Assert
        //     Assert.Equal(2, _pageModel.Reservations.Count); // Only today's and future reservations for "testUserId" should be included
        //     Assert.Contains(_pageModel.Reservations, r => r.Date == today.ToString("yyyy-MM-dd"));
        //     Assert.Contains(_pageModel.Reservations, r => r.Date == today.AddDays(1).ToString("yyyy-MM-dd"));
        //     Assert.DoesNotContain(_pageModel.Reservations, r => r.Date == today.AddDays(-1).ToString("yyyy-MM-dd"));
        //     Assert.DoesNotContain(_pageModel.Reservations, r => r.CustomerId == "anotherUserId");
        // }
    }
}
