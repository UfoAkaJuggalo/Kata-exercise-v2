Feature: Links

Ability to add the clickable links to messages

Scenario: Add link to message
	Given user name is "Alice"
	And message with text "it's just a test" and link "https://github.com/" with description "Github"
	When User publish the message
	Then message should be added to user timeline with links
