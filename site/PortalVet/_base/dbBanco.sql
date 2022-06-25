-- --------------------------------------------------------
-- Servidor:                     mysql.webimagem.vet.br
-- Versão do servidor:           10.2.38-MariaDB - mariadb.org binary distribution
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Copiando estrutura do banco de dados para h3223_webimagem
CREATE DATABASE IF NOT EXISTS `h3223_webimagem` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `h3223_webimagem`;

-- Copiando estrutura para tabela h3223_webimagem.adminmenu
CREATE TABLE IF NOT EXISTS `adminmenu` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Ordernumber` int(11) NOT NULL,
  `Path` varchar(100) NOT NULL,
  `Module` varchar(100) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `Icon` varchar(100) NOT NULL,
  `AreaName` varchar(100) DEFAULT NULL,
  `ControllerName` varchar(100) DEFAULT NULL,
  `ActionName` varchar(100) DEFAULT NULL,
  `Active` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminmenu: ~15 rows (aproximadamente)
/*!40000 ALTER TABLE `adminmenu` DISABLE KEYS */;
INSERT INTO `adminmenu` (`Id`, `Name`, `Ordernumber`, `Path`, `Module`, `ParentId`, `Icon`, `AreaName`, `ControllerName`, `ActionName`, `Active`) VALUES
	(1, 'Painel Inicial', 10, '', 'Dashboard', 0, 'font-icon-speed', NULL, 'Dashboard', 'Index', b'1'),
	(2, 'Administração', 20, '', '', 0, 'font-icon-cogwheel', NULL, NULL, NULL, b'1'),
	(3, 'Perfil', 10, '', 'Perfis', 2, '', 'Administracao', 'Perfil', 'Index', b'1'),
	(4, 'Usuário', 20, '', 'Usuarios', 2, '', 'Administracao', 'User', 'Index', b'1'),
	(5, 'Menu', 30, '', 'Menus', 2, '', 'Administracao', 'Menu', 'Index', b'1'),
	(6, 'Cadastros', 30, '', '', 0, 'font-icon-cogwheel', NULL, NULL, NULL, b'1'),
	(7, 'Proprietários', 20, '', '', 6, '', 'Cadastro', 'Cliente', 'Index', b'1'),
	(8, 'Unidades', 50, '', '', 2, '', 'Cadastro', 'Clinica', 'Index', b'1'),
	(9, 'Gerentes', 20, '', '', 6, '', 'Cadastro', 'Gerente', 'Index', b'1'),
	(10, 'Exames', 10, '', '', 6, '', 'Cadastro', 'Exame', 'Index', b'1'),
	(11, 'Meus Exames', 20, '', '', 0, 'font-icon font-icon-post', '', 'MeusExames', 'Index', b'1'),
	(12, 'Laudadores', 20, '', '', 6, '', 'Cadastro', 'Laudador', 'Index', b'1'),
	(13, 'Exames', 10, '', '', 6, '', 'Cadastro', 'Exame', 'Index', b'1'),
	(14, 'Exames', 10, '', '', 6, '', 'Cadastro', 'Exame', 'Index', b'1'),
	(15, 'Modelos de Exame', 20, '', '', 6, '', 'Cadastro', 'ContratoModelo', 'Index', b'1');
/*!40000 ALTER TABLE `adminmenu` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminmenuteste
CREATE TABLE IF NOT EXISTS `adminmenuteste` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MenuId` int(11) NOT NULL,
  `EmpresaId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminmenuteste: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `adminmenuteste` DISABLE KEYS */;
/*!40000 ALTER TABLE `adminmenuteste` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminpermission
CREATE TABLE IF NOT EXISTS `adminpermission` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `RoleId` bigint(20) NOT NULL,
  `ProfileId` bigint(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=119 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminpermission: ~80 rows (aproximadamente)
/*!40000 ALTER TABLE `adminpermission` DISABLE KEYS */;
INSERT INTO `adminpermission` (`Id`, `RoleId`, `ProfileId`) VALUES
	(1, 1, 1),
	(2, 2, 1),
	(3, 3, 1),
	(4, 4, 1),
	(5, 5, 1),
	(6, 6, 1),
	(7, 7, 1),
	(8, 8, 1),
	(9, 9, 1),
	(10, 10, 1),
	(11, 11, 1),
	(12, 12, 1),
	(13, 13, 1),
	(14, 14, 1),
	(15, 15, 1),
	(16, 16, 1),
	(17, 17, 1),
	(18, 18, 1),
	(19, 19, 1),
	(20, 20, 1),
	(31, 1, 3),
	(32, 1, 2),
	(33, 28, 2),
	(34, 27, 2),
	(35, 22, 2),
	(36, 24, 2),
	(37, 23, 2),
	(38, 25, 2),
	(39, 30, 2),
	(40, 29, 2),
	(46, 25, 3),
	(47, 26, 3),
	(48, 24, 3),
	(49, 23, 3),
	(51, 38, 3),
	(52, 39, 3),
	(53, 37, 3),
	(54, 40, 3),
	(61, 26, 2),
	(62, 41, 2),
	(69, 21, 2),
	(70, 42, 1),
	(71, 43, 1),
	(72, 46, 1),
	(73, 44, 1),
	(74, 45, 1),
	(75, 50, 2),
	(76, 51, 2),
	(77, 48, 2),
	(78, 49, 2),
	(79, 47, 2),
	(80, 1, 5),
	(84, 55, 3),
	(85, 56, 3),
	(86, 53, 3),
	(87, 54, 3),
	(88, 52, 3),
	(89, 60, 5),
	(90, 61, 5),
	(91, 58, 5),
	(92, 59, 5),
	(93, 57, 5),
	(94, 21, 5),
	(95, 62, 2),
	(96, 64, 2),
	(97, 63, 2),
	(98, 66, 2),
	(99, 65, 2),
	(106, 62, 5),
	(107, 63, 5),
	(108, 64, 5),
	(109, 65, 5),
	(110, 66, 5),
	(112, 21, 3),
	(113, 4, 2),
	(114, 42, 2),
	(115, 46, 2),
	(116, 43, 2),
	(117, 45, 2),
	(118, 44, 2);
/*!40000 ALTER TABLE `adminpermission` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminprofile
CREATE TABLE IF NOT EXISTS `adminprofile` (
  `Id` int(11) DEFAULT NULL,
  `Nome` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminprofile: ~5 rows (aproximadamente)
/*!40000 ALTER TABLE `adminprofile` DISABLE KEYS */;
INSERT INTO `adminprofile` (`Id`, `Nome`) VALUES
	(1, 'Administrador'),
	(2, 'Gerente'),
	(3, 'Clínica'),
	(5, 'Laudador');
/*!40000 ALTER TABLE `adminprofile` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminrole
CREATE TABLE IF NOT EXISTS `adminrole` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Key` varchar(100) DEFAULT NULL,
  `Page` varchar(100) DEFAULT NULL,
  `MenuId` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=67 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminrole: ~66 rows (aproximadamente)
/*!40000 ALTER TABLE `adminrole` DISABLE KEYS */;
INSERT INTO `adminrole` (`Id`, `Name`, `Key`, `Page`, `MenuId`) VALUES
	(1, 'Ver Menu', 'VER_MENU', 'Index', '1'),
	(2, 'Atualizar', 'ATUALIZAR', 'Profile', '1'),
	(3, 'Listar', 'LISTAR', 'Perfil', '1'),
	(4, 'Ver Menu', 'VER_MENU', 'Index', '2'),
	(5, 'Ver Menu', 'VER_MENU', 'Index', '3'),
	(6, 'Atualizar', 'ATUALIZAR', 'Save', '3'),
	(7, 'Cadastrar', 'CADASTRAR', 'Save', '3'),
	(8, 'Listar', 'LISTAR', 'Index', '3'),
	(9, 'Remover', 'REMOVER', 'Index', '3'),
	(10, 'Ver Menu', 'VER_MENU', 'Index', '4'),
	(11, 'Atualizar', 'ATUALIZAR', 'Save', '4'),
	(12, 'Cadastrar', 'CADASTRAR', 'Save', '4'),
	(13, 'Editar Topo', 'EDITAR_TOPO', 'Edit', '4'),
	(14, 'Listar', 'LISTAR', 'Index', '4'),
	(15, 'Remover', 'REMOVER', 'Index', '4'),
	(16, 'Ver Menu', 'VER_MENU', 'Index', '5'),
	(17, 'Atualizar', 'ATUALIZAR', 'Save', '5'),
	(18, 'Cadastrar', 'CADASTRAR', 'Save', '5'),
	(19, 'Listar', 'LISTAR', 'Index', '5'),
	(20, 'Remover', 'REMOVER', 'Index', '5'),
	(21, 'Ver Menu', 'VER_MENU', 'Index', '6'),
	(22, 'Ver Menu', 'VER_MENU', 'Index', '7'),
	(23, 'Remover', 'REMOVER', 'Index', '7'),
	(24, 'Listar', 'LISTAR', 'Index', '7'),
	(25, 'Atualizar', 'ATUALIZAR', 'Save', '7'),
	(26, 'Cadastrar', 'CADASTRAR', 'Save', '7'),
	(27, 'Listar', 'LISTAR', 'Index', '10'),
	(28, 'Ver Menu', 'VER_MENU', 'Index', '10'),
	(29, 'Atualizar', 'ATUALIZAR', 'Save', '10'),
	(30, 'Remover', 'REMOVER', 'Index', '10'),
	(31, 'Ver Menu', 'VER_MENU', 'Index', '11'),
	(32, 'Listar', 'LISTAR', 'Index', '11'),
	(33, 'Remover', 'REMOVER', 'Index', '11'),
	(34, 'Atualizar', 'ATUALIZAR', 'Save', '11'),
	(35, 'Cadastrar', 'CADASTRAR', 'Save', '11'),
	(36, 'Ver Menu', 'VER_MENU', 'Index', '9'),
	(37, 'Listar', 'LISTAR', 'Index', '9'),
	(38, 'Atualizar', 'ATUALIZAR', 'Save', '9'),
	(39, 'Cadastrar', 'CADASTRAR', 'Save', '9'),
	(40, 'Remover', 'REMOVER', 'Index', '9'),
	(41, 'Cadastrar', 'CADASTRAR', 'Save', '10'),
	(42, 'Ver Menu', 'VER_MENU', 'Index', '8'),
	(43, 'Listar', 'LISTAR', 'Index', '8'),
	(44, 'Atualizar', 'ATUALIZAR', 'Save', '8'),
	(45, 'Cadastrar', 'CADASTRAR', 'Save', '8'),
	(46, 'Remover', 'REMOVER', 'Index', '8'),
	(47, 'Ver Menu', 'VER_MENU', 'Index', '12'),
	(48, 'Listar', 'LISTAR', 'Index', '12'),
	(49, 'Remover', 'REMOVER', 'Index', '12'),
	(50, 'Atualizar', 'ATUALIZAR', 'Save', '12'),
	(51, 'Cadastrar', 'CADASTRAR', 'Save', '12'),
	(52, 'Ver Menu', 'VER_MENU', 'Index', '13'),
	(53, 'Listar', 'LISTAR', 'Index', '13'),
	(54, 'Remover', 'REMOVER', 'Index', '13'),
	(55, 'Atualizar', 'ATUALIZAR', 'Save', '13'),
	(56, 'Cadastrar', 'CADASTRAR', 'Save', '13'),
	(57, 'Ver Menu', 'VER_MENU', 'Index', '14'),
	(58, 'Listar', 'LISTAR', 'Index', '14'),
	(59, 'Remover', 'REMOVER', 'Index', '14'),
	(60, 'Atualizar', 'ATUALIZAR', 'Save', '14'),
	(61, 'Cadastrar', 'CADASTRAR', 'Save', '14'),
	(62, 'Ver Menu', 'VER_MENU', 'Index', '15'),
	(63, 'Listar', 'LISTAR', 'Index', '15'),
	(64, 'Remover', 'REMOVER', 'Index', '15'),
	(65, 'Atualizar', 'ATUALIZAR', 'Save', '15'),
	(66, 'Cadastrar', 'CADASTRAR', 'Save', '15');
/*!40000 ALTER TABLE `adminrole` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminuser
CREATE TABLE IF NOT EXISTS `adminuser` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CPFCNPJ` varchar(100) DEFAULT NULL,
  `Nome` varchar(100) DEFAULT NULL,
  `Sobrenome` varchar(100) DEFAULT NULL,
  `Email` varchar(100) NOT NULL,
  `Imagem` varchar(100) DEFAULT NULL,
  `Telefone` varchar(100) DEFAULT NULL,
  `Telefone2` varchar(100) DEFAULT NULL,
  `Senha` varchar(100) NOT NULL,
  `Ativo` bit(1) DEFAULT NULL,
  `DtCriacao` datetime DEFAULT NULL,
  `CodigoClinicaOfflineID` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminuser: ~11 rows (aproximadamente)
/*!40000 ALTER TABLE `adminuser` DISABLE KEYS */;
INSERT INTO `adminuser` (`Id`, `CPFCNPJ`, `Nome`, `Sobrenome`, `Email`, `Imagem`, `Telefone`, `Telefone2`, `Senha`, `Ativo`, `DtCriacao`, `CodigoClinicaOfflineID`) VALUES
	(1, '', 'Leandro', 'Fonseca', 'leandrofonseca.ti@gmail.com', NULL, '(51) 9998-12345', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-04-27 13:59:41', NULL),
	(2, '', 'Luciano', 'Santos', 'luciano@webimagem.vet.br', NULL, '(51) 9790-2653', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-04-27 13:59:41', NULL),
	(3, '', 'Fernando', 'Malta', 'fernando@webimagem.vet.br', NULL, '(51) 8461-8423', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-04-27 13:59:41', NULL),
	(6, NULL, 'GERENTE', 'Empresa', 'gerente@radmovel.com.br', NULL, '(31) 2312-312312', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-05-10 20:21:04', NULL),
	(7, NULL, 'GERENTE', 'Empresa', 'gerente@ultramovel.com.br', NULL, '(12) 3123-123123', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-05-10 20:21:45', NULL),
	(10, NULL, 'Laudador1', 'bla', 'laudador@teste.com', NULL, '(31231) 2312-31231', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-05-13 14:13:35', NULL),
	(11, NULL, 'Laudador2', 'ttt', 'le.fonseca2@gmail.com', NULL, '(123) 1231-23123', NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-05-20 16:23:10', NULL),
	(19, NULL, 'Gerente', 'RadImovel', 'gerente@radimovel.com.br', NULL, NULL, NULL, '4297f44b13955235245b2497399d7a93', b'1', '2021-06-21 18:31:42', NULL),
	(24, NULL, 'LEA KIRST', NULL, 'le.fonseca@gmail.com', NULL, '', NULL, '47626', b'1', '2021-09-28 15:42:39', 476),
	(25, NULL, 'VETMAX', NULL, 'FERNANDO.MALTA@OUTLOOK.COM', NULL, '51984729529', NULL, '52156', b'1', '2022-03-11 20:21:56', 521),
	(26, NULL, 'AGRO MAYER', NULL, 'RADMOVEL.VET@GMAIL.COM', NULL, '51984618423', NULL, '69bc27993c87c5c95e7305e1d2cd2327', b'1', '2022-03-11 23:40:56', 558);
/*!40000 ALTER TABLE `adminuser` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminusercompany
CREATE TABLE IF NOT EXISTS `adminusercompany` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) DEFAULT NULL,
  `CompanyId` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminusercompany: ~35 rows (aproximadamente)
/*!40000 ALTER TABLE `adminusercompany` DISABLE KEYS */;
INSERT INTO `adminusercompany` (`Id`, `UserId`, `CompanyId`) VALUES
	(21, 6, 2),
	(22, 7, 3),
	(25, 4, 2),
	(26, 4, 3),
	(28, 0, 1),
	(41, 13, 1),
	(42, 14, 1),
	(43, 15, 1),
	(44, 16, 6),
	(45, 17, 1),
	(46, 18, 6),
	(47, 19, 6),
	(49, 5, 1),
	(51, 8, 1),
	(54, 20, 1),
	(55, 21, 1),
	(56, 22, 1),
	(57, 2, 1),
	(58, 2, 6),
	(59, 2, 2),
	(60, 3, 1),
	(61, 3, 6),
	(62, 3, 2),
	(66, 23, 1),
	(67, 24, 1),
	(68, 25, 1),
	(69, 26, 1),
	(70, 1, 1),
	(71, 1, 2),
	(75, 10, 1),
	(76, 10, 6),
	(77, 10, 2),
	(78, 11, 1),
	(79, 11, 6),
	(80, 11, 2);
/*!40000 ALTER TABLE `adminusercompany` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.adminuserprofile
CREATE TABLE IF NOT EXISTS `adminuserprofile` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ProfileId` bigint(20) NOT NULL,
  `UserId` bigint(20) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.adminuserprofile: ~27 rows (aproximadamente)
/*!40000 ALTER TABLE `adminuserprofile` DISABLE KEYS */;
INSERT INTO `adminuserprofile` (`Id`, `ProfileId`, `UserId`) VALUES
	(23, 2, 6),
	(24, 2, 7),
	(26, 4, 4),
	(35, 4, 9),
	(37, 4, 12),
	(38, 5, 13),
	(39, 5, 14),
	(42, 2, 19),
	(43, 4, 17),
	(45, 2, 5),
	(47, 3, 8),
	(50, 3, 20),
	(51, 3, 21),
	(52, 3, 22),
	(53, 1, 2),
	(54, 2, 2),
	(55, 1, 3),
	(56, 2, 3),
	(57, 3, 3),
	(60, 3, 23),
	(61, 3, 24),
	(62, 3, 25),
	(63, 3, 26),
	(64, 1, 1),
	(65, 2, 1),
	(69, 5, 10),
	(70, 5, 11);
/*!40000 ALTER TABLE `adminuserprofile` ENABLE KEYS */;

-- Copiando estrutura para função h3223_webimagem.BUSCA_PARENT_MENU
DELIMITER //
CREATE FUNCTION `BUSCA_PARENT_MENU`(`p_MENUID` INT) RETURNS longtext CHARSET latin1
BEGIN
DECLARE v_AUX VARCHAR(100);
  
DECLARE v_RESULT LONGTEXT DEFAULT '';
DECLARE v_pid INT DEFAULT 0;

set v_pid = (SELECT PARENTID FROM AdminMenu where ID = p_MENUID);

WHILE v_pid >  0
DO
	SET v_AUX = (SELECT `NAME` FROM AdminMenu where ID = v_pid);

	if(v_RESULT = '')
	then
		SET v_RESULT = v_AUX;
	else
		SET v_RESULT =  CONCAT(v_AUX , ' / ' , v_RESULT);
	end if;
  
	set v_pid = (SELECT PARENTID FROM AdminMenu where ID = v_pid);
END WHILE;
 
 
   RETURN v_RESULT;
END//
DELIMITER ;

-- Copiando estrutura para tabela h3223_webimagem.cliente
CREATE TABLE IF NOT EXISTS `cliente` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExameId` bigint(20) NOT NULL DEFAULT 0,
  `CPFCNPJ` varchar(100) DEFAULT NULL,
  `Nome` varchar(100) DEFAULT NULL,
  `Email` varchar(100) NOT NULL,
  `Imagem` varchar(100) DEFAULT NULL,
  `Telefone` varchar(100) DEFAULT NULL,
  `DtCriacao` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.cliente: ~1 rows (aproximadamente)
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` (`Id`, `ExameId`, `CPFCNPJ`, `Nome`, `Email`, `Imagem`, `Telefone`, `DtCriacao`) VALUES
	(25, 0, '1111', 'FERNANDO', 'FERNANDO.MALTA@OUTLOOK.COM', NULL, '51984618423', '2022-03-11 20:59:29');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.clientecompany
CREATE TABLE IF NOT EXISTS `clientecompany` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) DEFAULT NULL,
  `CompanyId` bigint(20) DEFAULT NULL,
  `ExameID` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.clientecompany: ~1 rows (aproximadamente)
/*!40000 ALTER TABLE `clientecompany` DISABLE KEYS */;
INSERT INTO `clientecompany` (`Id`, `UserId`, `CompanyId`, `ExameID`) VALUES
	(68, 25, 1, NULL);
/*!40000 ALTER TABLE `clientecompany` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.clinica
CREATE TABLE IF NOT EXISTS `clinica` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `UnidadeId` bigint(20) DEFAULT NULL,
  `Codigo` bigint(20) DEFAULT NULL,
  `Nome` varchar(100) DEFAULT NULL,
  `Telefone` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Responsavel` varchar(100) DEFAULT NULL,
  `Responsavel2` varchar(100) DEFAULT NULL,
  `Endereco` varchar(200) DEFAULT NULL,
  `Ativo` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.clinica: ~84 rows (aproximadamente)
/*!40000 ALTER TABLE `clinica` DISABLE KEYS */;
INSERT INTO `clinica` (`Id`, `UnidadeId`, `Codigo`, `Nome`, `Telefone`, `Email`, `Responsavel`, `Responsavel2`, `Endereco`, `Ativo`) VALUES
	(1, NULL, NULL, 'ImagemMóvel', '', '', '', '', '', b'1'),
	(2, NULL, NULL, 'Radmóvel', '', '', '', '', '', b'1'),
	(3, NULL, NULL, 'Ultramóvel', '', '', '', '', '', b'1'),
	(5, 1, 557, 'DR DE BICHO', NULL, NULL, NULL, NULL, NULL, b'1'),
	(6, 1, 521, 'VETMAX', NULL, NULL, NULL, NULL, NULL, NULL),
	(7, 1, 514, 'VET CARE', NULL, NULL, NULL, NULL, NULL, NULL),
	(8, 1, 435, 'CHATTERIE', NULL, NULL, NULL, NULL, NULL, NULL),
	(9, 1, 475, 'KREFTA', NULL, NULL, NULL, NULL, NULL, NULL),
	(10, 1, 418, 'ANIMED RS', NULL, NULL, NULL, NULL, NULL, NULL),
	(11, 1, 506, 'SALVAR', NULL, NULL, NULL, NULL, NULL, NULL),
	(12, 1, 422, 'BICHO POINT', NULL, NULL, NULL, NULL, NULL, NULL),
	(13, 1, 452, 'DR LEONARDO', NULL, NULL, NULL, NULL, NULL, NULL),
	(14, 1, 456, 'DR RAUL', NULL, NULL, NULL, NULL, NULL, NULL),
	(15, 1, 469, 'FLOR DA TERRA', NULL, NULL, NULL, NULL, NULL, NULL),
	(16, 1, 477, 'LEAL VET', NULL, NULL, NULL, NULL, NULL, NULL),
	(17, 1, 520, 'VETMAIS', NULL, NULL, NULL, NULL, NULL, NULL),
	(18, 1, 446, 'DISK DOG', NULL, NULL, NULL, NULL, NULL, NULL),
	(19, 1, 425, 'BIG DOGS BRASIL', NULL, NULL, NULL, NULL, NULL, NULL),
	(20, 1, 424, 'BICHOS DA TERRA', NULL, NULL, NULL, NULL, NULL, NULL),
	(21, 1, 519, 'VETERINARIA DA TERRA', NULL, NULL, NULL, NULL, NULL, NULL),
	(22, 1, 482, 'MATRIZ', NULL, NULL, NULL, NULL, NULL, NULL),
	(23, 1, 420, 'BICHO DE LUXO', NULL, NULL, NULL, NULL, NULL, NULL),
	(24, 1, 551, 'ALEGRIA DOS BICHOS', NULL, NULL, NULL, NULL, NULL, NULL),
	(25, 1, 462, 'DRA LILIANE', NULL, NULL, NULL, NULL, NULL, NULL),
	(26, 1, 488, 'PARTICULAR', NULL, NULL, NULL, NULL, NULL, NULL),
	(27, 1, 500, 'PET SPERK CACHOEIRINHA', NULL, NULL, NULL, NULL, NULL, NULL),
	(28, 1, 433, 'CENV', NULL, NULL, NULL, NULL, NULL, NULL),
	(29, 1, 470, 'FMMA', NULL, NULL, NULL, NULL, NULL, NULL),
	(30, 1, 451, 'DR FABIO VET CARE', NULL, NULL, NULL, NULL, NULL, NULL),
	(31, 1, 460, 'DRA DE BICHO', NULL, NULL, NULL, NULL, NULL, NULL),
	(32, 1, 498, 'PET MOVEL', NULL, NULL, NULL, NULL, NULL, NULL),
	(33, 1, 415, 'AMIGO TRI LEGAL', NULL, NULL, NULL, NULL, NULL, NULL),
	(34, 1, 552, 'AMIGO ANIMAL', NULL, NULL, NULL, NULL, NULL, NULL),
	(35, 1, 525, 'ZOOMED', NULL, NULL, NULL, NULL, NULL, NULL),
	(36, 1, 501, 'PET SPERK GRAVATAI', NULL, NULL, NULL, NULL, NULL, NULL),
	(37, 1, 496, 'PET MAIS', NULL, NULL, NULL, NULL, NULL, NULL),
	(38, 1, 499, 'PET PUFF', NULL, NULL, NULL, NULL, NULL, NULL),
	(39, 1, 524, 'VETTIE', NULL, NULL, NULL, NULL, NULL, NULL),
	(40, 1, 493, 'PET ESTIMACAO', NULL, NULL, NULL, NULL, NULL, NULL),
	(41, 1, 504, 'PONTO DO CAO', NULL, NULL, NULL, NULL, NULL, NULL),
	(42, 1, 502, 'PLANETA AGROPECUARIA', NULL, NULL, NULL, NULL, NULL, NULL),
	(43, 1, 444, 'DA MATRIZ', NULL, NULL, NULL, NULL, NULL, NULL),
	(44, 1, 423, 'BICHO ZEN', NULL, NULL, NULL, NULL, NULL, NULL),
	(45, 1, 523, 'VETS SUPORT HOME CARE', NULL, NULL, NULL, NULL, NULL, NULL),
	(46, 1, 440, 'CLINICA VET ALVARO MUSSOI', NULL, NULL, NULL, NULL, NULL, NULL),
	(47, 1, 478, 'LINDINHOS E PERFUMADOS', NULL, NULL, NULL, NULL, NULL, NULL),
	(48, 1, 495, 'PET HOME', NULL, NULL, NULL, NULL, NULL, NULL),
	(49, 1, 497, 'PET MIX', NULL, NULL, NULL, NULL, NULL, NULL),
	(50, 1, 490, 'PERRO Y GATITO', NULL, NULL, NULL, NULL, NULL, NULL),
	(51, 1, 443, 'CUSCO AMIGO', NULL, NULL, NULL, NULL, NULL, NULL),
	(52, 1, 486, 'MY PET', NULL, NULL, NULL, NULL, NULL, NULL),
	(53, 1, 421, 'BICHO MANIA', NULL, NULL, NULL, NULL, NULL, NULL),
	(54, 1, 515, 'VET DA TERRA', NULL, NULL, NULL, NULL, NULL, NULL),
	(55, 1, 465, 'DUDU PET IPANEMA', NULL, NULL, NULL, NULL, NULL, NULL),
	(56, 1, 505, 'REDE ANIMALE', NULL, NULL, NULL, NULL, NULL, NULL),
	(57, 1, 457, 'DR RENATO', NULL, NULL, NULL, NULL, NULL, NULL),
	(58, 1, 522, 'VETPLEX', NULL, NULL, NULL, NULL, NULL, NULL),
	(59, 1, 474, 'IBAMA', NULL, NULL, NULL, NULL, NULL, NULL),
	(60, 1, 481, 'MASTER PET', NULL, NULL, NULL, NULL, NULL, NULL),
	(61, 1, 484, 'MIMOS PET', NULL, NULL, NULL, NULL, NULL, NULL),
	(62, 1, 508, 'SPA DOS MIMOS', NULL, NULL, NULL, NULL, NULL, NULL),
	(63, 1, 461, 'DRA INGRID', NULL, NULL, NULL, NULL, NULL, NULL),
	(64, 1, 459, 'DRA CASSIA ZAMBIASI', NULL, NULL, NULL, NULL, NULL, NULL),
	(65, 1, 509, 'STANIVET', NULL, NULL, NULL, NULL, NULL, NULL),
	(66, 1, 439, 'CLINICA DR LEONARDO', NULL, NULL, NULL, NULL, NULL, NULL),
	(67, 1, 437, 'CIA DOS BICHOS', NULL, NULL, NULL, NULL, NULL, NULL),
	(68, 1, 428, 'C.V.BARAO DO AMAZONAS', NULL, NULL, NULL, NULL, NULL, NULL),
	(69, 1, 429, 'C.V.SILVIO OLISESKI', NULL, NULL, NULL, NULL, NULL, NULL),
	(70, 1, 479, 'MANIA DE BICHO', NULL, NULL, NULL, NULL, NULL, NULL),
	(71, 1, 553, 'AGROPET', NULL, NULL, NULL, NULL, NULL, NULL),
	(72, 1, 448, 'DR ALVARO', NULL, NULL, NULL, NULL, NULL, NULL),
	(73, 1, 491, 'PET BARBER', NULL, NULL, NULL, NULL, NULL, NULL),
	(74, 1, 503, 'PLURAL PATAS', NULL, NULL, NULL, NULL, NULL, NULL),
	(75, 1, 411, 'ALOHA', NULL, NULL, NULL, NULL, NULL, NULL),
	(76, 1, 518, 'VETBURKI', NULL, NULL, NULL, NULL, NULL, NULL),
	(77, 1, 546, 'SARA FACCHIN', NULL, NULL, NULL, NULL, NULL, NULL),
	(78, 1, 487, 'NOSSO CANTINHO', NULL, NULL, NULL, NULL, NULL, NULL),
	(79, 1, 412, 'ALVARO MUSSOI', NULL, NULL, NULL, NULL, NULL, NULL),
	(80, 1, 558, 'AGRO MAYER', NULL, NULL, NULL, NULL, NULL, NULL),
	(81, 1, 431, 'CENTRAL ANIMAL', NULL, NULL, NULL, NULL, NULL, NULL),
	(82, 1, 560, 'AGROPAL', NULL, NULL, NULL, NULL, NULL, NULL),
	(83, 1, 468, 'FLAVIA KREWER', NULL, NULL, NULL, NULL, NULL, NULL),
	(84, 1, 554, 'CLINICA VET DONNA VET', NULL, NULL, NULL, NULL, NULL, NULL),
	(85, 1, 562, 'DUDU CLINICA 24H', NULL, NULL, NULL, NULL, NULL, NULL),
	(86, 6, 476, 'LEA KIRST', '51987654321', 'leandrofonseca.ti@gmail.com', NULL, NULL, NULL, NULL);
/*!40000 ALTER TABLE `clinica` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.company
CREATE TABLE IF NOT EXISTS `company` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Imagem` varchar(100) DEFAULT NULL,
  `Texto` text DEFAULT NULL,
  `Ativo` bit(1) NOT NULL,
  `Chave` varchar(50) DEFAULT uuid(),
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.company: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` (`Id`, `Nome`, `Imagem`, `Texto`, `Ativo`, `Chave`) VALUES
	(1, 'ImageMóvel', 'file20210527185233325.jpeg', 'Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor. Nullam quis risus eget urna mollis ornare vel eu leo. Cras justo odio, dapibus ac facilisis in, egestas eget quam. teste', b'1', '500086ac-c853-11eb-9877-00155d5a576a'),
	(2, 'UltraMóvel', 'file20210527185302323.jpeg', '.. Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor. Nullam quis risus eget urna mollis ornare vel eu leo. Cras justo odio, dapibus ac facilisis in, egestas eget quam.', b'1', '6bd46b69-c853-11eb-9877-00155d5a576a'),
	(6, 'RadMóvel', 'file20210527185250710.jpeg', '... Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor. Nullam quis risus eget urna mollis ornare vel eu leo. Cras justo odio, dapibus ac facilisis in, egestas eget quam.', b'1', '741e7151-c853-11eb-9877-00155d5a576a');
/*!40000 ALTER TABLE `company` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.documentomodelo
CREATE TABLE IF NOT EXISTS `documentomodelo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CompanyId` int(11) NOT NULL DEFAULT 0,
  `LaudadorId` int(11) NOT NULL DEFAULT 0,
  `Nome` varchar(150) DEFAULT NULL,
  `Perfil` varchar(150) DEFAULT NULL,
  `ModeloCabecalho` text DEFAULT NULL,
  `ModeloCorpo` text DEFAULT NULL,
  `ModeloRodape` text DEFAULT NULL,
  `DtCadastro` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.documentomodelo: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `documentomodelo` DISABLE KEYS */;
INSERT INTO `documentomodelo` (`Id`, `CompanyId`, `LaudadorId`, `Nome`, `Perfil`, `ModeloCabecalho`, `ModeloCorpo`, `ModeloRodape`, `DtCadastro`) VALUES
	(1, 1, 0, 'MODELO 01', NULL, NULL, '<p>Região: Craniana  </p><p>Projeções: DV e Lateral  <br>Achados Radiográficos:  <br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão,  <br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas,  <br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes,  <br>- cavidade nasal, seio frontal e bulas timpânicas limpas,  <br>- elementos dentários preservados,  <br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>', '<p style="text-align:center;">Médico veterinário </p><p style="text-align:center;">LUCIANO DA COSTA SANTOS - CRMV/RS 10917 </p><p style="text-align:center;">E-mail: imagemovel@imagemovel.vet.br </p><p style="text-align:center;">Cel: (051) 98470-2653</p>', '2021-05-27 15:45:57'),
	(2, 1, 10, 'teste 01', NULL, NULL, '<p>tetetette dsasdfa asdf asfasd</p>', '<p>f dsa fsadf sad fsadf </p>', '2022-03-27 22:16:42'),
	(3, 1, 10, 'teste 2', NULL, NULL, '<p>asdfasdf asdf asdf sadfasd dfsadf </p>', '<p>fsadfsd fasdf safd</p>', '2022-03-27 22:16:58');
/*!40000 ALTER TABLE `documentomodelo` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.documentomodeloversao
CREATE TABLE IF NOT EXISTS `documentomodeloversao` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `TipoModuloId` int(11) DEFAULT NULL,
  `ModeloId` int(11) DEFAULT NULL,
  `ModuloId` int(11) DEFAULT NULL,
  `CompanyId` int(11) DEFAULT NULL,
  `ModeloCorpo` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.documentomodeloversao: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `documentomodeloversao` DISABLE KEYS */;
/*!40000 ALTER TABLE `documentomodeloversao` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.documentovariavel
CREATE TABLE IF NOT EXISTS `documentovariavel` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(150) DEFAULT NULL,
  `Descricao` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.documentovariavel: ~13 rows (aproximadamente)
/*!40000 ALTER TABLE `documentovariavel` DISABLE KEYS */;
INSERT INTO `documentovariavel` (`Id`, `Nome`, `Descricao`) VALUES
	(1, 'DATA_EXAME', '-'),
	(2, 'EXAME_IDADE_PACIENTE', '-'),
	(3, 'EXAME_VETERINARIO', '-'),
	(4, 'EXAME_PROPRIETARIO', '-'),
	(5, 'EXAME_PACIENTE', '-'),
	(6, 'EXAME_ESPECIE', '-'),
	(7, 'CLIENTE_NOME', '-'),
	(8, 'CLIENTE_EMAIL', '-'),
	(9, 'CLIENTE_CPF_CNPJ', '-'),
	(10, 'LAUDADOR_NOME', '-'),
	(11, 'LAUDADOR_EMAIL', '-'),
	(12, 'LAUDADOR_CPF_CNPJ', '-'),
	(13, 'CLINICA_NOME', '-');
/*!40000 ALTER TABLE `documentovariavel` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.especie
CREATE TABLE IF NOT EXISTS `especie` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Ordenacao` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.especie: ~6 rows (aproximadamente)
/*!40000 ALTER TABLE `especie` DISABLE KEYS */;
INSERT INTO `especie` (`Id`, `Nome`, `Ordenacao`) VALUES
	(1, 'Felinos', 1),
	(2, 'Caninos', 2),
	(3, 'Aves', 3),
	(4, 'Repteis', 4),
	(5, 'Roedores', 5),
	(6, 'Outros', 10);
/*!40000 ALTER TABLE `especie` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.exame
CREATE TABLE IF NOT EXISTS `exame` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Codigo` bigint(20) DEFAULT NULL,
  `DataExame` datetime NOT NULL,
  `SituacaoId` int(11) DEFAULT NULL,
  `CompanyId` int(11) DEFAULT NULL,
  `ClinicaId` int(11) DEFAULT NULL,
  `LaudadorSituacaoId` int(11) DEFAULT NULL,
  `SituacaoNome` varchar(50) DEFAULT NULL,
  `Veterinario` varchar(100) DEFAULT NULL,
  `Paciente` varchar(100) DEFAULT NULL,
  `Proprietario` varchar(200) DEFAULT NULL,
  `ProprietarioEmail` varchar(200) DEFAULT NULL,
  `ProprietarioTelefone` varchar(200) DEFAULT NULL,
  `Idade` varchar(100) DEFAULT NULL,
  `Valor` varchar(100) DEFAULT NULL,
  `FormaPagamento` varchar(100) DEFAULT NULL,
  `Historico` mediumtext DEFAULT NULL,
  `ClienteId` int(11) DEFAULT NULL,
  `RacaOutros` varchar(100) DEFAULT NULL,
  `EspecieSelecao` varchar(100) DEFAULT NULL,
  `RacaSelecao` varchar(100) DEFAULT NULL,
  `LaudadorId` int(11) DEFAULT NULL,
  `DataVinculoLaudador` datetime DEFAULT NULL,
  `DataCadastro` datetime NOT NULL,
  `DataAtualizacao` datetime NOT NULL,
  `Descricao` mediumtext DEFAULT NULL,
  `Rodape` mediumtext DEFAULT NULL,
  `RacaId` int(11) DEFAULT NULL,
  `EspecieOutros` varchar(100) DEFAULT NULL,
  `EspecieId` int(11) DEFAULT NULL,
  `ArquivadoLaudador` bit(1) NOT NULL,
  `ArquivadoGerente` bit(1) NOT NULL,
  `ArquivadoClinica` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=820 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.exame: ~6 rows (aproximadamente)
/*!40000 ALTER TABLE `exame` DISABLE KEYS */;
INSERT INTO `exame` (`Id`, `Codigo`, `DataExame`, `SituacaoId`, `CompanyId`, `ClinicaId`, `LaudadorSituacaoId`, `SituacaoNome`, `Veterinario`, `Paciente`, `Proprietario`, `ProprietarioEmail`, `ProprietarioTelefone`, `Idade`, `Valor`, `FormaPagamento`, `Historico`, `ClienteId`, `RacaOutros`, `EspecieSelecao`, `RacaSelecao`, `LaudadorId`, `DataVinculoLaudador`, `DataCadastro`, `DataAtualizacao`, `Descricao`, `Rodape`, `RacaId`, `EspecieOutros`, `EspecieId`, `ArquivadoLaudador`, `ArquivadoGerente`, `ArquivadoClinica`) VALUES
	(814, 5916, '2022-03-11 20:00:00', 3, 1, 25, NULL, 'Em Análise (Clínica)', '', 'FRAJOLA', '11', '222', '333', '13A', '250', 'DINHEIRO', 'tosse', 25, '', 'FELINO', 'SRD', 0, NULL, '2022-03-11 20:59:29', '0000-00-00 00:00:00', NULL, NULL, 175, 'FELINO', 6, b'0', b'0', b'0'),
	(815, 5917, '2022-03-11 20:00:00', 3, 1, 25, NULL, 'Em Análise (Clínica)', '', 'BELA', '44', '55', '666', '12A', '250', 'PIX', 'atropelado', 25, '', 'FELINO', 'SRD', 0, NULL, '2022-03-11 20:59:31', '0000-00-00 00:00:00', NULL, NULL, 175, 'FELINO', 6, b'0', b'0', b'0'),
	(816, 0, '2022-03-11 22:00:00', 3, 1, 25, NULL, 'Em Análise (Laudador)', 'vett', 'LULU', 'LUCIANO', 'LUCIANO.CS.VET@GMAIL.COM', '51', '5A', 'luciano@webimagem.vet.br', 'PIX', 'tosse..', 0, NULL, 'CANINO', 'DACHSHUND', 10, '2022-03-27 10:10:59', '2022-03-11 22:14:53', '2022-03-28 10:41:36', '<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>', '<p style="text-align:center;">Médico veterinário&nbsp;</p><p style="text-align:center;">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style="text-align:center;">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style="text-align:center;">Cel: (051) 98470-2653</p>', 0, '', 0, b'0', b'0', b'0'),
	(817, 5919, '2022-03-11 22:00:00', 3, 1, 25, NULL, 'Em Análise (Clínica)', '', 'TOTÓ', 'LUCIANO', 'LUCIANO.CS.VET@GMAIL.COM', '51', '', NULL, NULL, '', 0, '', '', '', 10, '2022-03-27 22:17:16', '2022-03-11 22:31:57', '0000-00-00 00:00:00', NULL, NULL, 293, '', 6, b'0', b'0', b'0'),
	(818, 5920, '2022-03-11 22:00:00', 5, 1, 25, NULL, 'Concluído (Gerente)', '', 'KAUA', 'LUCIANINHO', 'LUCIANO.CS.VET@GMAIL.COM', '51', '2A', NULL, NULL, 'vsfvfvfv', 0, '', 'FELINO', 'EUROPEU', 11, '2022-03-28 10:26:12', '2022-03-11 22:46:21', '0000-00-00 00:00:00', '', '', 153, 'FELINO', 6, b'0', b'0', b'0'),
	(819, 0, '2022-03-11 23:00:00', 6, 1, 26, NULL, 'Concluído', '', 'BICHO DOS INFERNOS', 'LUCIANINHO DO SESC', 'LUCIANO.CS.VET@GMAIL.COM', '51', '6A', 'luciano@webimagem.vet.br', '', 'ggbgb', 0, NULL, 'CANINO', 'YORKSHIRE TERRIER', 11, '2022-03-27 21:45:11', '2022-03-11 23:40:56', '0000-00-00 00:00:00', '', '', 0, '', 0, b'0', b'0', b'0');
/*!40000 ALTER TABLE `exame` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.examehistorico
CREATE TABLE IF NOT EXISTS `examehistorico` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExameId` bigint(20) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `UsuarioNome` varchar(100) NOT NULL,
  `UsuarioEmail` varchar(100) NOT NULL,
  `Descricao` text DEFAULT NULL,
  `Conteudo` text DEFAULT NULL,
  `SituacaoId` int(11) DEFAULT NULL,
  `DataCadastro` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1016 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.examehistorico: ~110 rows (aproximadamente)
/*!40000 ALTER TABLE `examehistorico` DISABLE KEYS */;
INSERT INTO `examehistorico` (`Id`, `ExameId`, `UsuarioId`, `UsuarioNome`, `UsuarioEmail`, `Descricao`, `Conteudo`, `SituacaoId`, `DataCadastro`) VALUES
	(906, 814, 0, 'API', '', 'Atualização dos dados.', '{"Id":814,"Codigo":5916,"DataExame":"2022-03-11T20:00:00","DataExameFmt":"11/03/2022","DataExameHH":"20","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":25,"ClinicaId":25,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":175,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"13A","Paciente":"FRAJOLA","EspecieNome":null,"EspecieOutros":"FELINO","EspecieId":6,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":null,"Rodape":null,"Historico":"tosse","Valor":"250","FormaPagamento":"DINHEIRO","EspecieSelecao":"FELINO","RacaSelecao":"SRD","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 20:59:29'),
	(907, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:30'),
	(908, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:31'),
	(909, 815, 0, 'API', '', 'Atualização dos dados.', '{"Id":815,"Codigo":5917,"DataExame":"2022-03-11T20:00:00","DataExameFmt":"11/03/2022","DataExameHH":"20","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":25,"ClinicaId":25,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":175,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"12A","Paciente":"BELA","EspecieNome":null,"EspecieOutros":"FELINO","EspecieId":6,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":null,"Rodape":null,"Historico":"atropelado","Valor":"250","FormaPagamento":"PIX","EspecieSelecao":"FELINO","RacaSelecao":"SRD","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 20:59:31'),
	(910, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:31'),
	(911, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:32'),
	(912, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:32'),
	(913, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 20:59:32'),
	(914, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:51'),
	(915, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:51'),
	(916, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:51'),
	(917, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:52'),
	(918, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:52'),
	(919, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:52'),
	(920, 816, 0, 'API', '', 'Atualização dos dados.', '{"Id":816,"Codigo":5918,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":185,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"CANINO","EspecieId":6,"Proprietario":"LUCIANO","ProprietarioEmail":"LUCIANO.CS.VET@GMAIL.COM","ProprietarioTelefone":"51","Descricao":null,"Rodape":null,"Historico":"tosse","Valor":"250","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 22:14:53'),
	(921, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:54'),
	(922, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:54'),
	(923, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:14:54'),
	(924, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:54'),
	(925, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:55'),
	(926, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:55'),
	(927, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:55'),
	(928, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:56'),
	(929, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:56'),
	(930, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:57'),
	(931, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:57'),
	(932, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:57'),
	(933, 817, 0, 'API', '', 'Atualização dos dados.', '{"Id":817,"Codigo":5919,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":293,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"","Paciente":"TOTÓ","EspecieNome":null,"EspecieOutros":"","EspecieId":6,"Proprietario":"LUCIANO","ProprietarioEmail":"LUCIANO.CS.VET@GMAIL.COM","ProprietarioTelefone":"51","Descricao":null,"Rodape":null,"Historico":"","Valor":"","FormaPagamento":"","EspecieSelecao":"","RacaSelecao":"","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 22:31:57'),
	(934, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:58'),
	(935, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:31:58'),
	(936, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:18'),
	(937, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:18'),
	(938, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:19'),
	(939, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:19'),
	(940, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:19'),
	(941, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:20'),
	(942, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:20'),
	(943, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:20'),
	(944, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:20'),
	(945, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:21'),
	(946, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:21'),
	(947, 818, 0, 'API', '', 'Atualização dos dados.', '{"Id":818,"Codigo":5920,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":153,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"2A","Paciente":"KAUA","EspecieNome":null,"EspecieOutros":"FELINO","EspecieId":6,"Proprietario":"LUCIANINHO","ProprietarioEmail":"LUCIANO.CS.VET@GMAIL.COM","ProprietarioTelefone":"51","Descricao":null,"Rodape":null,"Historico":"vsfvfvfv","Valor":"","FormaPagamento":"","EspecieSelecao":"FELINO","RacaSelecao":"EUROPEU","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 22:46:21'),
	(948, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:24'),
	(949, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:25'),
	(950, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:25'),
	(951, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 22:46:25'),
	(952, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:51'),
	(953, 814, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:51'),
	(954, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:52'),
	(955, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:52'),
	(956, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:52'),
	(957, 815, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:53'),
	(958, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:53'),
	(959, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:53'),
	(960, 816, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:54'),
	(961, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:54'),
	(962, 817, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:54'),
	(963, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:54'),
	(964, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:55'),
	(965, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:55'),
	(966, 818, 0, 'API', '', 'Atualização dos dados (Imagens).', '', 1, '2022-03-11 23:40:56'),
	(967, 819, 0, 'API', '', 'Atualização dos dados.', '{"Id":819,"Codigo":5921,"DataExame":"2022-03-11T23:00:00","DataExameFmt":"11/03/2022","DataExameHH":"23","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":"","ClienteId":0,"ClinicaId":26,"LaudadorId":0,"NomeCliente":"","NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":246,"RacaNome":null,"RacaOutros":"","Veterinario":"","Idade":"6A","Paciente":"BICHO DOS INFERNOS","EspecieNome":null,"EspecieOutros":"CANINO","EspecieId":6,"Proprietario":"LUCIANINHO DO SESC","ProprietarioEmail":"LUCIANO.CS.VET@GMAIL.COM","ProprietarioTelefone":"51","Descricao":null,"Rodape":null,"Historico":"ggbgb","Valor":"","FormaPagamento":"","EspecieSelecao":"CANINO","RacaSelecao":"YORKSHIRE TERRIER","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 1, '2022-03-11 23:40:56'),
	(968, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"","Idade":"5A,LUCIANO,LUCIANO.CS.VET@GMAIL.COM,51","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse","Valor":"250","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:28:14'),
	(969, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A,LUCIANO,LUCIANO.CS.VET@GMAIL.COM,51,LUCIANO,LUCIANO.CS.VET@GMAIL.COM,51","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse","Valor":"250","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:28:21'),
	(970, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A,LUCIANO,LUCIANO.CS.VET@GMAIL.COM,51","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:32:26'),
	(971, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:32:43'),
	(972, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:32:58'),
	(973, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":0,"SituacaoNome":null,"EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 0, '2022-03-26 23:34:22'),
	(974, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 2, '2022-03-26 23:34:32'),
	(975, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 2, '2022-03-26 23:36:27'),
	(976, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":2,"SituacaoNome":"Em Análise (Clínica)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 2, '2022-03-26 23:47:18'),
	(977, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-26 23:47:37'),
	(978, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 00:07:42'),
	(979, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"teste","Rodape":"","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 00:53:39'),
	(980, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:31:17'),
	(981, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:31:49'),
	(982, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:32:28'),
	(983, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:32:31'),
	(984, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:33:33'),
	(985, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:33:36'),
	(986, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:34:20'),
	(987, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:34:24'),
	(988, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:34:39'),
	(989, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:34:42'),
	(990, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:34:48'),
	(991, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:36:07'),
	(992, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:36:09'),
	(993, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:54:05'),
	(994, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, desvinculado com exame : 816.', '', 3, '2022-03-27 01:58:33'),
	(995, 816, 11, 'Laudador Teste', 'le.fonseca@gmail.com', 'Laudador : Laudador Teste, vinculado com exame : 816.', '', 3, '2022-03-27 01:59:43'),
	(996, 816, 10, 'Laudador', 'laudador@teste.com', 'Laudador : Laudador, vinculado com exame : 816.', '', 3, '2022-03-27 02:03:50'),
	(997, 816, 10, 'Laudador', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"teste asdf asdf&nbsp;","Rodape":"","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 02:06:11'),
	(998, 816, 10, 'Laudador', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"teste asdf asdf&nbsp; asdfsadfasdf asdf asfd","Rodape":"fff","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 02:06:27'),
	(999, 816, 10, 'Laudador1', 'laudador@teste.com', 'Laudador : Laudador1, desvinculado com exame : 816.', '', 3, '2022-03-27 09:40:51'),
	(1000, 816, 10, 'Laudador1', 'laudador@teste.com', 'Laudador : Laudador1, vinculado com exame : 816.', '', 3, '2022-03-27 09:51:08'),
	(1001, 816, 10, 'Laudador1', 'laudador@teste.com', 'Laudo expirou, ficou disponível.', '', 3, '2022-03-27 09:59:04'),
	(1002, 816, 10, 'Laudador1', 'laudador@teste.com', 'Laudo expirou, ficou disponível.', '', 3, '2022-03-27 10:09:58'),
	(1003, 816, 10, 'Laudador1', 'laudador@teste.com', 'Laudador : Laudador1, vinculado com exame : 816.', '', 3, '2022-03-27 10:11:03'),
	(1004, 816, 10, 'Laudador1', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 10:11:16'),
	(1005, 816, 10, 'Laudador1', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 10:13:40'),
	(1006, 816, 10, 'Laudador1', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":5,"SituacaoNome":"Concluído (Gerente)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 5, '2022-03-27 10:13:47'),
	(1007, 816, 10, 'Laudador1', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-27 10:15:49'),
	(1008, 816, 10, 'Laudador1', 'laudador@teste.com', 'Atualização dos dados (Laudador).', '{"Id":816,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":5,"SituacaoNome":"Concluído (Gerente)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 5, '2022-03-27 10:18:43'),
	(1009, 819, 11, 'Laudador2', 'le.fonseca2@gmail.com', 'Laudador : Laudador2, vinculado com exame : 819.', '', 3, '2022-03-27 21:45:15'),
	(1010, 819, 11, 'Laudador2', 'le.fonseca2@gmail.com', 'Atualização dos dados (Laudador).', '{"Id":819,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":5,"SituacaoNome":"Concluído (Gerente)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 5, '2022-03-27 21:53:54'),
	(1011, 819, 1, 'Leandro', 'leandrofonseca.ti@gmail.com', 'Atualização dos dados.', '{"Id":819,"Codigo":0,"DataExame":"2022-03-11T23:00:00","DataExameFmt":"11/03/2022","DataExameHH":"23","DataExameMM":"00","SituacaoId":6,"SituacaoNome":"Concluído","EmailCliente":null,"ClienteId":0,"ClinicaId":26,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"","Idade":"6A","Paciente":"BICHO DOS INFERNOS","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":"ggbgb","Valor":"luciano@webimagem.vet.br","FormaPagamento":"","EspecieSelecao":"CANINO","RacaSelecao":"YORKSHIRE TERRIER","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 6, '2022-03-27 21:55:15'),
	(1012, 817, 10, 'Laudador1', 'laudador@teste.com', 'Laudador : Laudador1, vinculado com exame : 817.', '', 3, '2022-03-27 22:17:20'),
	(1013, 818, 11, 'Laudador2', 'le.fonseca2@gmail.com', 'Laudador : Laudador2, vinculado com exame : 818.', '', 3, '2022-03-28 10:26:18'),
	(1014, 818, 11, 'Laudador2', 'le.fonseca2@gmail.com', 'Atualização dos dados (Laudador).', '{"Id":818,"Codigo":0,"DataExame":"0001-01-01T00:00:00","DataExameFmt":"01/01/0001","DataExameHH":"00","DataExameMM":"00","SituacaoId":5,"SituacaoNome":"Concluído (Gerente)","EmailCliente":null,"ClienteId":0,"ClinicaId":0,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":null,"Idade":null,"Paciente":null,"EspecieNome":null,"EspecieOutros":null,"EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"","Rodape":"","Historico":null,"Valor":null,"FormaPagamento":null,"EspecieSelecao":null,"RacaSelecao":null,"ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 5, '2022-03-28 10:26:45'),
	(1015, 816, 2, 'Luciano', 'luciano@webimagem.vet.br', 'Atualização dos dados.', '{"Id":816,"Codigo":0,"DataExame":"2022-03-11T22:00:00","DataExameFmt":"11/03/2022","DataExameHH":"22","DataExameMM":"00","SituacaoId":3,"SituacaoNome":"Em Análise (Laudador)","EmailCliente":null,"ClienteId":0,"ClinicaId":25,"LaudadorId":0,"NomeCliente":null,"NomeLaudador":null,"LaudadorSituacaoId":0,"LaudadorSituacaoNome":null,"CompanyId":1,"CompanyNome":null,"DataCadastro":"0001-01-01T00:00:00","DataVinculoLaudador":null,"PeriodoTermino":null,"PeriodoTerminoFmt":null,"PeriodoTermino2Fmt":null,"RacaId":0,"RacaNome":null,"RacaOutros":null,"Veterinario":"vett","Idade":"5A","Paciente":"LULU","EspecieNome":null,"EspecieOutros":"","EspecieId":0,"Proprietario":null,"ProprietarioEmail":null,"ProprietarioTelefone":null,"Descricao":"<p>Região: Craniana &nbsp;</p><p>Projeções: DV e Lateral &nbsp;<br>Achados Radiográficos: &nbsp;<br>- Moderado afastamento de desalinhamento da sínfise mandibular cranial, com aumento de volume de tecidos moles adjacentes a lesão, &nbsp;<br>- demais estruturas ósseas e articulares da região craniana alinhadas e preservadas, &nbsp;<br>- prolapso do globo ocular do lado esquerdo com aumento de volume de tecidos moles adjacentes, &nbsp;<br>- cavidade nasal, seio frontal e bulas timpânicas limpas, &nbsp;<br>- elementos dentários preservados, &nbsp;<br>- sem mais alterações dignas de notas nas imagens avaliadas.</p>","Rodape":"<p style=\\"text-align:center;\\">Médico veterinário&nbsp;</p><p style=\\"text-align:center;\\">LUCIANO DA COSTA SANTOS - CRMV/RS 10917&nbsp;</p><p style=\\"text-align:center;\\">E-mail: imagemovel@imagemovel.vet.br&nbsp;</p><p style=\\"text-align:center;\\">Cel: (051) 98470-2653</p>","Historico":"tosse..","Valor":"luciano@webimagem.vet.br","FormaPagamento":"PIX","EspecieSelecao":"CANINO","RacaSelecao":"DACHSHUND","ListHistorico":[],"LinkExame":null,"Duvidas":[]}', 3, '2022-03-28 10:41:41');
/*!40000 ALTER TABLE `examehistorico` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.examehistoricoduvida
CREATE TABLE IF NOT EXISTS `examehistoricoduvida` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExameId` bigint(20) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `UsuarioNome` varchar(100) NOT NULL,
  `UsuarioEmail` varchar(100) NOT NULL,
  `Tipo` varchar(100) NOT NULL,
  `Mensagem` mediumtext DEFAULT NULL,
  `DataCadastro` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=912 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.examehistoricoduvida: ~3 rows (aproximadamente)
/*!40000 ALTER TABLE `examehistoricoduvida` DISABLE KEYS */;
INSERT INTO `examehistoricoduvida` (`Id`, `ExameId`, `UsuarioId`, `UsuarioNome`, `UsuarioEmail`, `Tipo`, `Mensagem`, `DataCadastro`) VALUES
	(903, 818, 25, 'VETMAX', 'FERNANDO.MALTA@OUTLOOK.COM', '', 'sfgsdfgsdfsg', '2022-03-11 23:54:09'),
	(904, 816, 25, 'VETMAX', 'FERNANDO.MALTA@OUTLOOK.COM', '', 'teste 001', '2022-03-26 22:22:15'),
	(905, 818, 11, 'Laudador2', 'le.fonseca2@gmail.com', '', 'teste 2', '2022-03-28 10:26:27'),
	(906, 818, 2, 'Luciano', 'luciano@webimagem.vet.br', 'CLINICA', 'teste para CLINICA', '2022-03-28 21:48:16'),
	(908, 818, 2, 'Luciano', 'luciano@webimagem.vet.br', 'LAUDADOR', 'teste para LAUDADOR', '2022-03-28 22:21:12'),
	(909, 818, 2, 'Luciano', 'luciano@webimagem.vet.br', 'CLINICA', 'teste 2 para CLINICA', '2022-03-28 22:21:26'),
	(910, 818, 25, 'VETMAX', 'FERNANDO.MALTA@OUTLOOK.COM', 'CLINICA', 'teste CLINICA para GERENTE', '2022-03-28 22:22:58'),
	(911, 818, 11, 'Laudador2', 'le.fonseca2@gmail.com', 'LAUDADOR', 'LAUDADOR para GERENTE', '2022-03-28 22:26:03');
/*!40000 ALTER TABLE `examehistoricoduvida` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.exameimagehistorico
CREATE TABLE IF NOT EXISTS `exameimagehistorico` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ExameId` bigint(20) NOT NULL,
  `Resultado` bit(1) NOT NULL DEFAULT b'0',
  `UnidadeId` int(11) NOT NULL,
  `Location` varchar(300) DEFAULT NULL,
  `Exception` varchar(500) DEFAULT NULL,
  `FileName` varchar(100) NOT NULL,
  `Ext` varchar(50) NOT NULL,
  `DataCadastro` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=922 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- Copiando dados para a tabela h3223_webimagem.exameimagehistorico: ~56 rows (aproximadamente)
/*!40000 ALTER TABLE `exameimagehistorico` DISABLE KEYS */;
INSERT INTO `exameimagehistorico` (`Id`, `ExameId`, `Resultado`, `UnidadeId`, `Location`, `Exception`, `FileName`, `Ext`, `DataCadastro`) VALUES
	(866, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311205930434.jpg', '.jpg', '2022-03-11 20:59:31'),
	(867, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311205931106.jpg', '.jpg', '2022-03-11 20:59:31'),
	(868, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311205931809.jpg', '.jpg', '2022-03-11 20:59:31'),
	(869, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311205932231.jpg', '.jpg', '2022-03-11 20:59:32'),
	(870, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311205932543.jpg', '.jpg', '2022-03-11 20:59:32'),
	(871, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311205932872.jpg', '.jpg', '2022-03-11 20:59:32'),
	(872, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311221451125.jpg', '.jpg', '2022-03-11 22:14:51'),
	(873, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311221451250.jpg', '.jpg', '2022-03-11 22:14:51'),
	(874, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311221451765.jpg', '.jpg', '2022-03-11 22:14:51'),
	(875, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311221452171.jpg', '.jpg', '2022-03-11 22:14:52'),
	(876, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311221452515.jpg', '.jpg', '2022-03-11 22:14:52'),
	(877, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311221452937.jpg', '.jpg', '2022-03-11 22:14:53'),
	(878, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311221454281.jpg', '.jpg', '2022-03-11 22:14:54'),
	(879, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311221454500.jpg', '.jpg', '2022-03-11 22:14:54'),
	(880, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311221454671.jpg', '.jpg', '2022-03-11 22:14:54'),
	(881, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311223154977.jpg', '.jpg', '2022-03-11 22:31:55'),
	(882, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311223155102.jpg', '.jpg', '2022-03-11 22:31:55'),
	(883, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311223155524.jpg', '.jpg', '2022-03-11 22:31:55'),
	(884, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311223155883.jpg', '.jpg', '2022-03-11 22:31:55'),
	(885, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311223156227.jpg', '.jpg', '2022-03-11 22:31:56'),
	(886, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311223156634.jpg', '.jpg', '2022-03-11 22:31:56'),
	(887, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311223156977.jpg', '.jpg', '2022-03-11 22:31:57'),
	(888, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311223157196.jpg', '.jpg', '2022-03-11 22:31:57'),
	(889, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311223157368.jpg', '.jpg', '2022-03-11 22:31:57'),
	(890, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311223158618.jpg', '.jpg', '2022-03-11 22:31:58'),
	(891, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311223158758.jpg', '.jpg', '2022-03-11 22:31:58'),
	(892, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311224618540.jpg', '.jpg', '2022-03-11 22:46:18'),
	(893, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311224618649.jpg', '.jpg', '2022-03-11 22:46:18'),
	(894, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311224619040.jpg', '.jpg', '2022-03-11 22:46:19'),
	(895, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311224619384.jpg', '.jpg', '2022-03-11 22:46:19'),
	(896, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311224619712.jpg', '.jpg', '2022-03-11 22:46:19'),
	(897, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311224620102.jpg', '.jpg', '2022-03-11 22:46:20'),
	(898, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311224620415.jpg', '.jpg', '2022-03-11 22:46:20'),
	(899, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311224620697.jpg', '.jpg', '2022-03-11 22:46:20'),
	(900, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311224620853.jpg', '.jpg', '2022-03-11 22:46:20'),
	(901, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311224621088.jpg', '.jpg', '2022-03-11 22:46:21'),
	(902, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311224621228.jpg', '.jpg', '2022-03-11 22:46:21'),
	(903, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311224624651.jpg', '.jpg', '2022-03-11 22:46:24'),
	(904, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311224625026.jpg', '.jpg', '2022-03-11 22:46:25'),
	(905, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311224625494.jpg', '.jpg', '2022-03-11 22:46:25'),
	(906, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311224625885.jpg', '.jpg', '2022-03-11 22:46:25'),
	(907, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311234051330.jpg', '.jpg', '2022-03-11 23:40:51'),
	(908, 814, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\814', '', 'file_20220311234051565.jpg', '.jpg', '2022-03-11 23:40:51'),
	(909, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311234052002.jpg', '.jpg', '2022-03-11 23:40:52'),
	(910, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311234052315.jpg', '.jpg', '2022-03-11 23:40:52'),
	(911, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311234052690.jpg', '.jpg', '2022-03-11 23:40:52'),
	(912, 815, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\815', '', 'file_20220311234053051.jpg', '.jpg', '2022-03-11 23:40:53'),
	(913, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311234053379.jpg', '.jpg', '2022-03-11 23:40:53'),
	(914, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311234053613.jpg', '.jpg', '2022-03-11 23:40:53'),
	(915, 816, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\816', '', 'file_20220311234053972.jpg', '.jpg', '2022-03-11 23:40:54'),
	(916, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311234054238.jpg', '.jpg', '2022-03-11 23:40:54'),
	(917, 817, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\817', '', 'file_20220311234054535.jpg', '.jpg', '2022-03-11 23:40:54'),
	(918, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311234054910.jpg', '.jpg', '2022-03-11 23:40:54'),
	(919, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311234055316.jpg', '.jpg', '2022-03-11 23:40:55'),
	(920, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311234055785.jpg', '.jpg', '2022-03-11 23:40:55'),
	(921, 818, b'1', 1, 'D:\\website\\h3223\\public_site1\\Upload\\exames\\1\\818', '', 'file_20220311234056473.jpg', '.jpg', '2022-03-11 23:40:56');
/*!40000 ALTER TABLE `exameimagehistorico` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.mensagemmodelo
CREATE TABLE IF NOT EXISTS `mensagemmodelo` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CompanyId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `PerfilId` int(11) NOT NULL,
  `Perfil` varchar(150) DEFAULT NULL,
  `Titulo` varchar(150) DEFAULT NULL,
  `Mensagem` text DEFAULT NULL,
  `DataCriacao` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.mensagemmodelo: ~4 rows (aproximadamente)
/*!40000 ALTER TABLE `mensagemmodelo` DISABLE KEYS */;
INSERT INTO `mensagemmodelo` (`Id`, `CompanyId`, `UsuarioId`, `PerfilId`, `Perfil`, `Titulo`, `Mensagem`, `DataCriacao`) VALUES
	(1, 1, 5, 2, 'Gerente', 'MODELO 01', 'Olá {CLIENTE}, a clínica {CLINICA}, disponibilizou seu exame {URL_EXAME}', '2021-05-25 20:28:35'),
	(2, 3, 7, 2, 'Gerente', 'MODELO A', 'Olá {CLIENTE}, a clínica {CLINICA}, disponibilizou seu exame {URL_EXAME}.', '2021-05-25 21:23:25'),
	(3, 1, 5, 2, 'Gerente', 'MODELO 02', 'Olá {CLIENTE} asdfa sdf asdf asdf {CLINICA} asdfaasdfasd f', '2021-05-31 19:44:54'),
	(4, 1, 22, 3, 'Clínica', 'AVISO CLIENTE', 'Olá {CLIENTE}\nSEGUE A URL PARA ACESSAR {URL_EXAME}', '2021-09-10 20:14:18');
/*!40000 ALTER TABLE `mensagemmodelo` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.raca
CREATE TABLE IF NOT EXISTS `raca` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `EspecieId` bigint(20) NOT NULL DEFAULT 0,
  `Codigo` bigint(20) NOT NULL DEFAULT 0,
  `UnidadeId` bigint(20) NOT NULL DEFAULT 0,
  `Nome` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=294 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.raca: ~148 rows (aproximadamente)
/*!40000 ALTER TABLE `raca` DISABLE KEYS */;
INSERT INTO `raca` (`Id`, `EspecieId`, `Codigo`, `UnidadeId`, `Nome`) VALUES
	(146, 2, 7, 1, 'AFEGÃO HOUND'),
	(147, 2, 8, 1, 'AFFENPINSCHER'),
	(148, 2, 9, 1, 'AIREDALE TERRIER'),
	(149, 2, 10, 1, 'AKITA'),
	(150, 2, 11, 1, 'AMERICAN PIT BULL TERRIER'),
	(151, 2, 12, 1, 'AMERICAN STAFFORDSHIRE TERRIER'),
	(152, 2, 13, 1, 'BASENJI'),
	(153, 2, 14, 1, 'BASSET HOUND'),
	(154, 2, 15, 1, 'BEAGLE'),
	(155, 2, 16, 1, 'BEAGLE HARRIER'),
	(156, 2, 17, 1, 'BEARDED COLLIE'),
	(157, 2, 18, 1, 'BEDLINGTON TERRIER'),
	(158, 2, 19, 1, 'BERNESE MOUNTAIN DOG'),
	(159, 2, 20, 1, 'BICHON FRISÉ'),
	(160, 2, 21, 1, 'BLOODHOUND'),
	(161, 2, 22, 1, 'BOBTAIL'),
	(162, 2, 23, 1, 'BOIADEIRO AUSTRALIANO'),
	(163, 2, 24, 1, 'BOIADEIRO BERNÊS'),
	(164, 2, 25, 1, 'BORDER COLLIE'),
	(165, 2, 26, 1, 'BORDER TERRIER'),
	(166, 2, 27, 1, 'BORZOI'),
	(167, 2, 28, 1, 'BOSTON TERRIER'),
	(168, 2, 29, 1, 'BOXER'),
	(169, 2, 30, 1, 'BULDOGUE FRANCÊS'),
	(170, 2, 31, 1, 'BULDOGUE INGLÊS'),
	(171, 2, 32, 1, 'BULL TERRIER'),
	(172, 2, 33, 1, 'BULMASTIFE'),
	(173, 2, 34, 1, 'CAIRN TERRIER'),
	(174, 2, 35, 1, 'CANE CORSO'),
	(175, 2, 36, 1, 'CÃO DE ÁGUA PORTUGUÊS'),
	(176, 2, 37, 1, 'CÃO DE CRISTA CHINÊS'),
	(177, 2, 38, 1, 'CAVALIER KING CHARLES SPANIEL'),
	(178, 2, 39, 1, 'CHESAPEAKE BAY RETRIEVER'),
	(179, 2, 40, 1, 'CHIHUAHUA'),
	(180, 2, 41, 1, 'CHOW CHOW'),
	(181, 2, 42, 1, 'COCKER SPANIEL AMERICANO'),
	(182, 2, 43, 1, 'COCKER SPANIEL INGLÊS'),
	(183, 2, 44, 1, 'COLLIE'),
	(184, 2, 45, 1, 'COTON DE TULÉAR'),
	(185, 2, 46, 1, 'DACHSHUND'),
	(186, 2, 47, 1, 'DÁLMATA'),
	(187, 2, 48, 1, 'DANDIE DINMONT TERRIER'),
	(188, 2, 49, 1, 'DOBERMANN'),
	(189, 2, 50, 1, 'DOGO ARGENTINO'),
	(190, 2, 51, 1, 'DOGUE ALEMÃO'),
	(191, 2, 52, 1, 'FILA BRASILEIRO'),
	(192, 2, 53, 1, 'FOX TERRIER (PELO DURO E PELO LISO)'),
	(193, 2, 54, 1, 'FOXHOUND INGLÊS'),
	(194, 2, 55, 1, 'GALGO ESCOCÊS'),
	(195, 2, 56, 1, 'GALGO IRLANDÊS'),
	(196, 2, 57, 1, 'GOLDEN RETRIEVER'),
	(197, 2, 58, 1, 'GRANDE BOIADEIRO SUIÇO'),
	(198, 2, 59, 1, 'GREYHOUND'),
	(199, 2, 60, 1, 'GRIFO DA BÉLGICA'),
	(200, 2, 61, 1, 'HUSKY SIBERIANO'),
	(201, 2, 62, 1, 'JACK RUSSELL TERRIER'),
	(202, 2, 63, 1, 'KING CHARLES'),
	(203, 2, 64, 1, 'KOMONDOR'),
	(204, 2, 65, 1, 'LABRADOODLE'),
	(205, 2, 66, 1, 'LABRADOR RETRIEVER'),
	(206, 2, 67, 1, 'LAKELAND TERRIER'),
	(207, 2, 68, 1, 'LEONBERGER'),
	(208, 2, 69, 1, 'LHASA APSO'),
	(209, 2, 70, 1, 'LULU DA POMERÂNIA'),
	(210, 2, 71, 1, 'MALAMUTE DO ALASCA'),
	(211, 2, 72, 1, 'MALTÊS'),
	(212, 2, 73, 1, 'MASTIFE'),
	(213, 2, 74, 1, 'MASTIM NAPOLITANO'),
	(214, 2, 75, 1, 'MASTIM TIBETANO'),
	(215, 2, 76, 1, 'MINI POODLE'),
	(216, 2, 77, 1, 'NORFOLK TERRIER'),
	(217, 2, 78, 1, 'NORWICH TERRIER'),
	(218, 2, 79, 1, 'PAPILLON'),
	(219, 2, 80, 1, 'PASTOR ALEMÃO'),
	(220, 2, 81, 1, 'PASTOR AUSTRALIANO'),
	(221, 2, 82, 1, 'PASTOR BELGA'),
	(222, 2, 83, 1, 'PASTOR BRANCO SUÍÇO'),
	(223, 2, 84, 1, 'PEQUENÊS'),
	(224, 2, 85, 1, 'PINSCHER MINIATURA'),
	(225, 2, 86, 1, 'POODLE'),
	(226, 2, 87, 1, 'PUG'),
	(227, 2, 88, 1, 'ROTTWEILER'),
	(228, 2, 89, 1, 'SÃO BERNARDO'),
	(229, 2, 90, 1, 'SCHNAUZER'),
	(230, 2, 91, 1, 'SHIH TZU'),
	(231, 2, 92, 1, 'SILKY TERRIER'),
	(232, 2, 93, 1, 'SKYE TERRIER'),
	(233, 2, 94, 1, 'SPITZ'),
	(234, 2, 95, 1, 'SRD'),
	(235, 2, 96, 1, 'STAFFORDSHIRE BULL TERRIER'),
	(236, 2, 97, 1, 'TECKEL'),
	(237, 2, 98, 1, 'TERRA NOVA'),
	(238, 2, 99, 1, 'TERRIER ESCOCÊS'),
	(239, 2, 100, 1, 'TOSA'),
	(240, 2, 101, 1, 'WEIMARANER'),
	(241, 2, 102, 1, 'WELSH CORGI (CARDIGAN)'),
	(242, 2, 103, 1, 'WELSH CORGI (PEMBROKE)'),
	(243, 2, 104, 1, 'WEST HIGHLAND WHITE TERRIER'),
	(244, 2, 105, 1, 'WHIPPET'),
	(245, 2, 106, 1, 'XOLOITZCUINTLI'),
	(246, 2, 107, 1, 'YORKSHIRE TERRIER'),
	(247, 2, 108, 1, 'ALEMAO'),
	(248, 2, 109, 1, 'ALEMAO2'),
	(249, 2, 110, 1, 'TESTE'),
	(250, 2, 111, 1, 'TESTE2'),
	(251, 2, 112, 1, 'PIT BULL'),
	(252, 2, 113, 1, 'BULLDOG CAMPEIRO'),
	(253, 1, 4, 1, 'ASHERA'),
	(254, 1, 5, 1, 'AZUL RUSSO'),
	(255, 1, 6, 1, 'BALINÊS'),
	(256, 1, 7, 1, 'BENGAL'),
	(257, 1, 8, 1, 'BIRMANÊS'),
	(258, 1, 9, 1, 'BOMBAIM'),
	(259, 1, 11, 1, 'CHAUSIE'),
	(260, 1, 12, 1, 'CORNISH REX'),
	(261, 1, 13, 1, 'DEVON REX'),
	(262, 1, 14, 1, 'EUROPEU'),
	(263, 1, 15, 1, 'EXÓTICO DE PELO CURTO'),
	(264, 1, 16, 1, 'HAVANA'),
	(265, 1, 17, 1, 'HIMALAIO'),
	(266, 1, 18, 1, 'JAVANÊS'),
	(267, 1, 19, 1, 'KORAT'),
	(268, 1, 20, 1, 'LYKOI'),
	(269, 1, 21, 1, 'MAINE COON'),
	(270, 1, 22, 1, 'MANX'),
	(271, 1, 23, 1, 'MAU EGÍPCIO'),
	(272, 1, 24, 1, 'MIST AUSTRALIANO'),
	(273, 1, 25, 1, 'MUNCHKIN'),
	(274, 1, 26, 1, 'NEBELUNG'),
	(275, 1, 27, 1, 'NORUEGUÊS DA FLORESTA'),
	(276, 1, 28, 1, 'OCICAT'),
	(277, 1, 29, 1, 'PETERBALD'),
	(278, 1, 30, 1, 'PERSA'),
	(279, 1, 31, 1, 'SAGRADO DA BIRMÂNIA'),
	(280, 1, 32, 1, 'SAVANNAH'),
	(281, 1, 33, 1, 'SCOTTISH FOLD'),
	(282, 1, 34, 1, 'SELKIRK REX'),
	(283, 1, 35, 1, 'SELVAGEM'),
	(284, 1, 36, 1, 'SRD'),
	(285, 1, 37, 1, 'SHORTHAIR'),
	(286, 1, 38, 1, 'SIAMÊS'),
	(287, 1, 39, 1, 'SIBERIANO'),
	(288, 1, 40, 1, 'SNOWSHOE'),
	(289, 1, 41, 1, 'SOKOKE'),
	(290, 1, 42, 1, 'SOMALI'),
	(291, 1, 43, 1, 'SPHYNX'),
	(292, 1, 44, 1, 'TURKISH VAN'),
	(293, 0, 0, 1, 'YORK');
/*!40000 ALTER TABLE `raca` ENABLE KEYS */;

-- Copiando estrutura para tabela h3223_webimagem.usercliente
CREATE TABLE IF NOT EXISTS `usercliente` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `adminUserId` bigint(20) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela h3223_webimagem.usercliente: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `usercliente` DISABLE KEYS */;
/*!40000 ALTER TABLE `usercliente` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
