<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$stepID = $_POST["StepName"];
$dentalDamInstall = $_POST["dentalDamInstall"];
$stepCompletTime = $_POST["StepCompletTime"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Use prepared statement to avoid SQL injection
$sql = "UPDATE installstep SET StepCompletTime = ?, StepDoneNumber = StepDoneNumber + 1 WHERE StepName = ? AND dentalDamInstall = ?";

// Prepare statement
$stmt = $conn->prepare($sql);

// Bind parameters: "s" for string (StepCompletTime), "s" for string (StepName), "i" for integer (dentalDamInstall)
$stmt->bind_param("ssi", $stepCompletTime, $stepID, $dentalDamInstall);

if ($stmt->execute()) {
    echo "Install step updated successfully. StepID: " . $stepID;
} else {
    echo "Error updating step: " . $stmt->error . "<br>";
}

$stmt->close(); // Close the statement
$conn->close(); // Close the connection
?>
