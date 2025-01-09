<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$installID = $_POST["installID"];
$completTime = $_POST["CompletTime"];
$user = $_POST["user"];  // User index

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Use prepared statement to avoid SQL injection
// Incrémenter SuccesNumber de 1
$sql = "UPDATE dentaldaminstall SET CompletTime = ?, SuccesNumber = SuccesNumber + 1, user = ? WHERE installID = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("sii", $completTime, $user, $installID); // "sii" means string, integer, integer parameters

if ($stmt->execute()) {
    echo "Pose updated successfully. InstallID: " . $installID;
} else {
    echo "Error updating pose: " . $stmt->error . "<br>";
}

$stmt->close(); // Close the statement
$conn->close(); // Close the connection
?>
