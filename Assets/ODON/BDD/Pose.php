<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$poseName = $_POST["poseName"];
$poseUser = $_POST["poseUser"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Use prepared statement to avoid SQL injection
$sql = "SELECT installID FROM dentaldaminstall WHERE installName = ? AND user = ?"; // Selection de installID

$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $poseName, $poseUser); // "ss" means two string parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Si la pose existe déjà, récupérer installID
    $row = $result->fetch_assoc();
    echo "Pose already made. Success : " . $row["installID"];
} else {
    // Si la pose n'existe pas, on insère une nouvelle ligne
    $sql2 = "INSERT INTO dentaldaminstall (installName, user) VALUES (?, ?)";

    $stmt2 = $conn->prepare($sql2);  // Initialiser $stmt2 ici seulement si nécessaire
    if ($stmt2) {  // Vérifier que $stmt2 a bien été créé
        $stmt2->bind_param("ss", $poseName, $poseUser); // Bind the parameters for the INSERT query

        if ($stmt2->execute()) {
            // Récupérer l'ID généré automatiquement par la base de données après l'insertion
            $installID = $conn->insert_id;
            echo "Success : " . $installID;
        } else {
            echo "Error: " . $stmt2->error . "<br>";
        }
        $stmt2->close(); // Fermeture de $stmt2 ici seulement si il a été initialisé
    } else {
        echo "Error preparing the statement for insertion.";
    }
}

$stmt->close(); // Close the first statement
$conn->close(); // Close the connection
?>
