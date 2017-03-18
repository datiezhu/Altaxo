﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

// Originated from: Roslyn, EditorFeatures, GoToDefinition/GoToDefinitionHelpers.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.FindUsages;
using Microsoft.CodeAnalysis.Navigation;
using Microsoft.CodeAnalysis.Options;
using Roslyn.Utilities;
using Microsoft.CodeAnalysis;

namespace Altaxo.CodeEditing.GoToDefinition
{
	public static class GoToDefinitionHelpers
	{
		public static bool TryGoToDefinition(
				ISymbol symbol,
				Project project,
				object /*IEnumerable<Lazy<IStreamingFindUsagesPresenter>> */ streamingPresenters,
				CancellationToken cancellationToken,
				bool thirdPartyNavigationAllowed = true,
				bool throwOnHiddenDefinition = false)
		{
			var alias = symbol as IAliasSymbol;
			if (alias != null)
			{
				var ns = alias.Target as INamespaceSymbol;
				if (ns != null && ns.IsGlobalNamespace)
				{
					return false;
				}
			}

			// VB global import aliases have a synthesized SyntaxTree.
			// We can't go to the definition of the alias, so use the target type.

			var solution = project.Solution;
			if (symbol is IAliasSymbol &&
					NavigableItemFactory.GetPreferredSourceLocations(solution, symbol).All(l => project.Solution.GetDocument(l.SourceTree) == null))
			{
				symbol = ((IAliasSymbol)symbol).Target;
			}

			var definition = SymbolFinder.FindSourceDefinitionAsync(symbol, solution, cancellationToken).WaitAndGetResult(cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();

			symbol = definition ?? symbol;

			var definitions = ArrayBuilder<DefinitionItem>.GetInstance();
			if (thirdPartyNavigationAllowed)
			{
				var factory = solution.Workspace.Services.GetService<IDefinitionsAndReferencesFactory>();
				var thirdPartyItem = factory?.GetThirdPartyDefinitionItem(solution, symbol);
				definitions.AddIfNotNull(thirdPartyItem);
			}

			// If it is a partial method declaration with no body, choose to go to the implementation
			// that has a method body.
			if (symbol is IMethodSymbol)
			{
				symbol = ((IMethodSymbol)symbol).PartialImplementationPart ?? symbol;
			}

			var options = project.Solution.Options;

			definitions.Add(symbol.ToDefinitionItem(solution, includeHiddenLocations: true));

			// var presenter = GetFindUsagesPresenter(streamingPresenters);
			// var title = string.Format(EditorFeaturesResources._0_declarations,FindUsagesHelpers.GetDisplayName(symbol));

			return IStreamingFindUsagesPresenterExtensions.TryNavigateToOrPresentItemsAsync(
				null,
				"Goto definition", // TODO find a title, see above
				 definitions.ToImmutableAndFree(),
					alwaysShowDeclarations: true).WaitAndGetResult(cancellationToken);
		}

		private static bool TryThirdPartyNavigation(ISymbol symbol, Solution solution)
		{
			var symbolNavigationService = solution.Workspace.Services.GetService<ISymbolNavigationService>();

			// Notify of navigation so third parties can intercept the navigation
			return symbolNavigationService.TrySymbolNavigationNotify(symbol, solution);
		}
	}
}