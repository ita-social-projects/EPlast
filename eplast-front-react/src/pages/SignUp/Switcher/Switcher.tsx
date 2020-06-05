import React from 'react';
import { useHistory } from 'react-router-dom';
// @ts-ignore
import styles from './Switcher.module.css';

export default function (props: { page: string }) {
  const history = useHistory();

  const { page } = props;

  const changePage = () => {
    if (page === 'SignUp') {
      return history.push('/signin');
    }
    return history.push('/signup');
  };

  return (
    <div className={styles.loginRegisterSwitcher}>
      <div className={styles.loginRectangle}>
        {page === 'SignUp' ? (
          <div className={styles.SwitcherText}>
            <p className={styles.loginText} onClick={changePage} onKeyDown={() => false} role="presentation">
              Увійти
            </p>
            <p id={styles.activeRegisterText} className={styles.registerText}>
              Зареєструватись
            </p>
            <div id={styles.registerActive} className={styles.activeRectangle} />
          </div>
        ) : (
          <div className={styles.SwitcherText}>
            <p id={styles.activeLoginText} className={styles.loginText}>
              Увійти
            </p>
            <p className={styles.registerText} onClick={changePage} onKeyDown={() => false} role="presentation">
              Зареєструватись
            </p>
            <div id={styles.loginActive} className={styles.activeRectangle} />
          </div>
        )}
      </div>
      <div className={styles.circleWrapper}>
        <div className={styles.loginCircle}>
          <p className={styles.orSwitcher}>АБО</p>
        </div>
      </div>
    </div>
  );
}
