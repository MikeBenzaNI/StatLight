﻿using Microsoft.Silverlight.Testing.Harness;
#if July2009 || October2009 || November2009
using Microsoft.Silverlight.Testing.UnitTesting.Harness;
#else
#endif
using Microsoft.Silverlight.Testing.UnitTesting.Metadata;
using StatLight.Client.Harness.Events;

namespace StatLight.Client.Harness.Hosts.MSTest.LogMessagTranslation
{
    public class TestExecutionClassBeginClientEventMap : ILogMessageToClientEventTranslation
    {
        public bool CanTranslate(LogMessage message)
        {
            if (message.MessageType == LogMessageType.TestExecution)
            {
                if (message.Is(TestStage.Starting)
                    && message.Is(TestGranularity.Test)
                    && message.DecoratorMatches(UnitTestLogDecorator.TestClassMetadata, v => v is ITestClass)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public ClientEvent Translate(LogMessage message)
        {
            var testClass = (ITestClass)message.Decorators[UnitTestLogDecorator.TestClassMetadata];
            var clientEventX = new TestExecutionClassBeginClientEvent
                                   {
                                       ClassName = testClass.Type.ClassNameIncludingParentsIfNested(),
                                       NamespaceName = testClass.Type.Namespace,
                                   };
            return clientEventX;
        }
    }
}