// <copyright file="PexAssemblyInfo.cs" company=""></copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("BeheerThemas")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("Microsoft.Practices.ObjectBuilder")]
[assembly: PexInstrumentAssembly("BeheerThemas.Interface")]
[assembly: PexInstrumentAssembly("Microsoft.Practices.CompositeWeb")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Practices.ObjectBuilder")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.Practices.CompositeWeb")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "BeheerThemas")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "BeheerThemas.Interface")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtStubSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]
[assembly: PexUseStubsFromTestAssembly]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]

[assembly: PexInstrumentAssembly("BeheerThemas.Tests")]
