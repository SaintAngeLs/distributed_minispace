import pytest
import responses
from test1 import TestFriendsAPI  

def setup_mocks():
    responses.start()
    # Mock the authentication endpoint
    responses.add(responses.POST, 'http://localhost:5004/sign-in',
                  json={'accessToken': 'mocked_token'}, status=200)
    # Mock the invite endpoint with a specific scenario
    responses.add(responses.POST, 'http://localhost:5012/friends/b202a5fe-afbd-4894-95e3-41aff99f430c/invite',
                  json={"error": "Already friends"}, status=400)

def teardown_mocks():
    responses.stop()
    responses.reset()

def run_tests():
    setup_mocks()
    result = pytest.main(['test1.py'])
    teardown_mocks()
    return result

if __name__ == '__main__':
    test_results = run_tests()
    print(f"Test Results: {test_results}")
