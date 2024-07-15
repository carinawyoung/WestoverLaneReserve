using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace WestoverLaneReserve.Tests.Helpers
{
    public static class TestHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => elements.Add(s));
            dbSetMock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => elements.Remove(s));

            return dbSetMock;
        }
    }
}
