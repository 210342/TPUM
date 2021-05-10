using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TPUM.Server.Data;
using Xunit;

namespace TPUM.Server.DataTests
{
    public class ObservableTest
    {
        public class TestEntity
        {
            public int Id { get; set; }
            public string Foo { get; set; }
        }

        private class ObservableImpl : Observable<TestEntity>
        {
            internal void InvokeEntityChangedForTests(TestEntity entity)
                => InvokeEntityChanged(entity);

            internal void InvokeEntitiesChangedForTests(IEnumerable<TestEntity> entity)
                => InvokeEntitiesChanged(entity);
        }

        [Fact]
        public void SubscribeTest()
        {
            Observable<TestEntity> sut = new ObservableImpl();
            Mock<IObserver<TestEntity>> observerMock = new();
            IDisposable sub = sut.Subscribe(observerMock.Object);
            Assert.NotNull(sub);
        }

        [Fact]
        public void SubscriptionEntityChangedTest()
        {
            ObservableImpl sut = new();
            Mock<IObserver<TestEntity>> observerMock = new();
            bool visited = false;
            observerMock.Setup(observer => observer.OnNext(It.IsAny<TestEntity>())).Callback(() => visited = true);
            IDisposable sub = sut.Subscribe(observerMock.Object);
            sut.InvokeEntityChangedForTests(new() { Id = 0 });
            Assert.True(visited);
        }

        [Fact]
        public void SubscriptionEntitiesChangedTest()
        {
            ObservableImpl sut = new();
            TestEntity entity1 = new() { Id = 0 };
            TestEntity entity2 = new() { Id = 1 };
            IEnumerable<TestEntity> entities = new[] { entity1, entity2, };
            Mock<IObserver<TestEntity>> observerMock = new();
            int visits = 0;
            observerMock.Setup(observer => observer.OnNext(It.IsAny<TestEntity>())).Callback(() => ++visits);
            IDisposable sub = sut.Subscribe(observerMock.Object);
            sut.InvokeEntitiesChangedForTests(entities);
            Assert.Equal(entities.Count(), visits);
        }

        [Fact]
        public void UnsubscribeTest()
        {
            ObservableImpl sut = new();
            TestEntity entity1 = new() { Id = 0 };
            TestEntity entity2 = new() { Id = 1 };
            IEnumerable<TestEntity> entities = new[] { entity1, entity2, };
            Mock<IObserver<TestEntity>> observerMock = new();
            int visits = 0;
            observerMock.Setup(observer => observer.OnNext(It.IsAny<TestEntity>())).Callback(() => ++visits);
            IDisposable sub = sut.Subscribe(observerMock.Object);
            sub.Dispose();
            sut.InvokeEntitiesChangedForTests(entities);
            Assert.Equal(0, visits);
        }

        [Fact]
        public void DisposeTest()
        {
            ObservableImpl sut = new();
            TestEntity entity1 = new() { Id = 0 };
            TestEntity entity2 = new() { Id = 1 };
            IEnumerable<TestEntity> entities = new[] { entity1, entity2, };
            Mock<IObserver<TestEntity>> observerMock = new();
            int visits = 0;
            observerMock.Setup(observer => observer.OnNext(It.IsAny<TestEntity>())).Callback(() => ++visits);
            IDisposable sub = sut.Subscribe(observerMock.Object);
            sut.Dispose();
            sut.InvokeEntitiesChangedForTests(entities);
            Assert.Equal(0, visits);
        }
    }
}
