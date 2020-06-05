import React from 'react';
import { Layout, Menu } from 'antd';
import { LoginOutlined } from '@ant-design/icons';
import { NavLink } from 'react-router-dom';
import LogoImg from '../../assets/images/ePlastLogotype.png';
import LogoText from '../../assets/images/logo_PLAST.svg';
import classes from './Header.module.css';

const HeaderContainer = () => {
  return (
    <Layout.Header className={classes.headerContainer}>
      <Menu mode="horizontal" className={classes.headerMenu}>
        <Menu.Item className={classes.headerItem} key="1">
          <div className={classes.headerLogo}>
            <NavLink to="/">
              <img src={LogoImg} alt="Logo" />
              <img src={LogoText} alt="Logo" />
            </NavLink>
          </div>
        </Menu.Item>
      </Menu>
      <Menu mode="horizontal" className={classes.headerMenu}>
        <Menu.Item className={classes.headerItem} key="2">
          <NavLink to="/contacts" className={classes.headerLink} activeClassName={classes.activeLink}>
            Контакти
          </NavLink>
        </Menu.Item>
        <Menu.Item className={classes.headerItem} key="3">
          <NavLink to="/signin" className={classes.headerLink} activeClassName={classes.activeLink}>
            Увійти
            <LoginOutlined className={classes.headerIcon} />
          </NavLink>
        </Menu.Item>
      </Menu>
    </Layout.Header>
  );
};
export default HeaderContainer;
