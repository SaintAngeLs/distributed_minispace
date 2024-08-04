from locust import HttpUser, TaskSet, task, between, events
import os

class UserBehavior(TaskSet):
    @task(1)
    def sign_in(self):
        self.client.post("/sign-in", json={"email": "austineccentric@gmail.com", "password": ""})

class WebsiteUser(HttpUser):
    tasks = [UserBehavior]
    wait_time = between(1, 2)

if __name__ == "__main__":
    # Default values
    default_user_count = 50000  # 1% of total users
    default_spawn_rate = default_user_count / 1800  # Spread the spawn over 30 minutes

    # Override with environment variables if available
    user_count = int(os.getenv("LOCUST_USERS", default_user_count))
    spawn_rate = float(os.getenv("LOCUST_SPAWN_RATE", default_spawn_rate))

    # Register environment variables
    @events.init_command_line_parser.add_listener
    def _(parser):
        parser.add_argument("--users", type=int, default=user_count, help="Number of concurrent users")
        parser.add_argument("--spawn-rate", type=float, default=spawn_rate, help="Rate to spawn users (users per second)")

    # Execute the test
    os.system(f"locust -f api-p-test.py --host http://localhost:5004 --users {user_count} --spawn-rate {spawn_rate}")
