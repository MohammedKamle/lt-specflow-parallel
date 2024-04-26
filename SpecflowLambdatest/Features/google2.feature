Feature: GoogleSearch

	Scenario: Goto Google
		Given goto Google
		Then title should be 'Google'