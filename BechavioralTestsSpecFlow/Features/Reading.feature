Feature: Reading
	Reading post from someone timeline


Scenario: Get post from someone timeline 
	Given the writer user name is Alice
	And the reader user name is Bob
	And Alice write messages on her timeline
	When Bob wants to see Alice timeline
	Then he gets all messages from Alice timeline