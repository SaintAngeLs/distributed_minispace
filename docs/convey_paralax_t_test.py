import numpy as np
from scipy import stats

response_times_convey = np.array([
    450.1734, 107.3799, 97.8681, 111.9321, 101.8884, 91.9052, 94.3284, 
    125.3144, 111.0173, 106.169, 96.4561, 113.2651, 136.9033, 95.6322, 129.1893
])

response_times_paralax = np.array([
    190.878, 100.3639, 105.5572, 111.9198, 124.3609, 105.2855, 104.1382, 
    115.2396, 143.0775, 94.5629, 96.1925, 95.7561, 100.2075, 112.5128, 110.4358
])

t_stat, p_value = stats.ttest_ind(response_times_convey, response_times_paralax)

print("Response Time Analysis between Convey and Paralax Frameworks")
print("-----------------------------------------------------------")
print(f"Mean response time for Convey: {np.mean(response_times_convey):.2f} ms")
print(f"Mean response time for Paralax: {np.mean(response_times_paralax):.2f} ms")
print("\nHypothesis Testing Results:")
print(f"T-statistic: {t_stat:.3f}")
print(f"P-value: {p_value:.3f}")

print("\nComparison of Response Times (ms):")
print("|   Trial   | Convey (ms) | Paralax (ms) |")
print("|-----------|-------------|--------------|")
for i in range(len(response_times_convey)):
    print(f"|     {i+1:<3}    | {response_times_convey[i]:<11} | {response_times_paralax[i]:<12} |")

alpha = 0.05  # Significance level
if p_value < alpha:
    print("Conclusion: Reject the null hypothesis (significant difference in response times).")
else:
    print("Conclusion: Fail to reject the null hypothesis (no significant difference in response times).")
