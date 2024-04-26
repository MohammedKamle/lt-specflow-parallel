Feature: GoogleSearch2

	Scenario: Search Google
		Given goto Google
		Then title should be 'Google'