Feature: Posting
	User can post messages to his own timeline

Scenario: Alice can publish messages to a personal timeline
	Given User named Alice
	And the message with text "it's just a test"
	When Alice publish the message
	Then the message should be added to Alice timeline