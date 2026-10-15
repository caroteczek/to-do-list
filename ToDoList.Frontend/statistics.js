async function loadStatistics() {
  const result = document.getElementById("statistics");
  result.textContent = "Ładowanie…";

  try {
    const response = await fetch("https://localhost:7232/api/Statistics");
    if (!response.ok) {
      result.textContent = `Nie udało się pobrać podsumowania (HTTP ${response.status}).`;
      return;
    }

    const data = await response.json();
    result.textContent = `Liczba zadań: ${data.taskCount}`;
  } catch (error) {
    result.textContent = "Nie udało się pobrać podsumowania: " + error.message;
  }
}
