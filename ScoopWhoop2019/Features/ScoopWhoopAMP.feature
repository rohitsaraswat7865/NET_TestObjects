Feature: ScoopWhoopAMP
	

@SWMobilesite
Scenario Outline: Check ScoopWhoop AMP is up and running
	Given Google home page loaded
	When Enter <ScoopWhoop> in google search bar on google home page
	And Click search button on google home page
	Then Redirected to <ScoopWhoop> amp search results
	And ScoopWhoop <ScoopWhoop> AMP elements are loaded
	And ScoopWhoop AMP elements are querable

	Examples:
	| ScoopWhoop       |
	| scoopwhoop       |
	