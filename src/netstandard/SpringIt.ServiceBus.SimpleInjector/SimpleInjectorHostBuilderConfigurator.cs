﻿using System;
using System.Collections.Generic;
using SimpleInjector;
using Topshelf.Builders;
using Topshelf.Configurators;
using Topshelf.HostConfigurators;

namespace SpringIt.ServiceBus.SimpleInjector
{
    public class SimpleInjectorHostBuilderConfigurator : HostBuilderConfigurator
    {
        #region Private Static Fields

        private static Container _container;

        #endregion

        #region Constructor

        public SimpleInjectorHostBuilderConfigurator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion

        #region Public Properties

        public static Container Container => _container;

        #endregion

        #region Public Methods

        public HostBuilder Configure(HostBuilder builder)
        {
            return builder;
        }

        public IEnumerable<ValidateResult> Validate()
        {
            yield break;
        }

        #endregion
    }
}