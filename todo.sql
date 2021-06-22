/*
 Navicat Premium Data Transfer

 Source Server         : todo
 Source Server Type    : MySQL
 Source Server Version : 80025
 Source Host           : localhost:3306
 Source Schema         : blog

 Target Server Type    : MySQL
 Target Server Version : 80025
 File Encoding         : 65001

 Date: 23/06/2021 00:45:45
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for todolist
-- ----------------------------
DROP TABLE IF EXISTS `todolist`;
CREATE TABLE `todolist`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Userid` int NULL DEFAULT NULL,
  `Datetime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `userid`(`Userid`) USING BTREE,
  CONSTRAINT `userid` FOREIGN KEY (`Userid`) REFERENCES `users` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 26 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Verification` int NULL DEFAULT NULL,
  `IsVerified` int NULL DEFAULT NULL,
  `PasswordHash` varbinary(255) NULL DEFAULT NULL,
  `PasswordSalt` varbinary(255) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
