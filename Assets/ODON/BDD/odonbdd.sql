-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : jeu. 07 nov. 2024 à 17:48
-- Version du serveur : 10.4.32-MariaDB
-- Version de PHP : 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `odonbdd`
--

-- --------------------------------------------------------

--
-- Structure de la table `dentaldaminstall`
--

CREATE TABLE `dentaldaminstall` (
  `installID` int(11) NOT NULL,
  `installName` varchar(40) NOT NULL,
  `CompletTime` time NOT NULL DEFAULT '00:00:00',
  `SuccesNumber` int(11) NOT NULL DEFAULT 0,
  `user` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `installstep`
--

CREATE TABLE `installstep` (
  `stepID` int(11) NOT NULL,
  `stepName` varchar(40) NOT NULL,
  `stepCompletTime` time NOT NULL DEFAULT '00:00:00',
  `StepDoneNumber` int(11) NOT NULL DEFAULT 0,
  `dentalDamInstall` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `stepmistake`
--

CREATE TABLE `stepmistake` (
  `mistakeID` int(11) NOT NULL,
  `mistakeName` varchar(40) NOT NULL,
  `mistakeDoneNumber` int(11) NOT NULL DEFAULT 1,
  `InstallStep` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE `user` (
  `UserID` int(11) NOT NULL,
  `UserName` varchar(20) NOT NULL,
  `UserMail` varchar(80) NOT NULL,
  `UserPassword` varchar(40) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`UserID`, `UserName`, `UserMail`, `UserPassword`) VALUES
(1, 'mdelm01', 'mathis.delmas@etud.univ-jfc.fr', 'test');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `dentaldaminstall`
--
ALTER TABLE `dentaldaminstall`
  ADD PRIMARY KEY (`installID`),
  ADD KEY `fk_install_user` (`user`);

--
-- Index pour la table `installstep`
--
ALTER TABLE `installstep`
  ADD PRIMARY KEY (`stepID`),
  ADD KEY `fk_step_install` (`dentalDamInstall`);

--
-- Index pour la table `stepmistake`
--
ALTER TABLE `stepmistake`
  ADD PRIMARY KEY (`mistakeID`),
  ADD KEY `fk_mistake_step` (`InstallStep`);

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UserID`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `dentaldaminstall`
--
ALTER TABLE `dentaldaminstall`
  MODIFY `installID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `installstep`
--
ALTER TABLE `installstep`
  MODIFY `stepID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `stepmistake`
--
ALTER TABLE `stepmistake`
  MODIFY `mistakeID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `dentaldaminstall`
--
ALTER TABLE `dentaldaminstall`
  ADD CONSTRAINT `fk_install_user` FOREIGN KEY (`user`) REFERENCES `user` (`UserID`);

--
-- Contraintes pour la table `installstep`
--
ALTER TABLE `installstep`
  ADD CONSTRAINT `fk_step_install` FOREIGN KEY (`dentalDamInstall`) REFERENCES `dentaldaminstall` (`installID`);

--
-- Contraintes pour la table `stepmistake`
--
ALTER TABLE `stepmistake`
  ADD CONSTRAINT `fk_mistake_step` FOREIGN KEY (`InstallStep`) REFERENCES `installstep` (`stepID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
