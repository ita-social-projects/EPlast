import React from 'react';
import { Layout } from 'antd';
import Facebook from '../../assets/images/facebook.svg';
import Twitter from '../../assets/images/bird.svg';
import Instagram from '../../assets/images/instagram.svg';
import classes from './Footer.module.css';

const FooterContainer = () => {
  return (
    <Layout.Footer className={classes.footerContainer}>
      <div className={classes.footerContacts}>
        <a href="https://www.facebook.com/PlastUA/">
          <img src={Facebook} alt="Facebook" />
        </a>
        <a href="https://twitter.com/plast">
          <img src={Twitter} alt="Twitter" />
        </a>
        <a href="https://www.instagram.com/plastua/">
          <img src={Instagram} alt="Instagram" />
        </a>
      </div>
      <p className={classes.footerCopyright}>ePlast © 2020</p>
    </Layout.Footer>
  );
};
export default FooterContainer;
