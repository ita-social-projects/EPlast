import React from 'react';
import { Layout, Carousel, Card } from 'antd';
import HomePict1 from '../../assets/images/homeMenuPicture.png';
import HomePict2 from '../../assets/images/homeMenuPicture(1).jpg';
import HomePict3 from '../../assets/images/homeMenuPicture(3).jpg';
import classes from './Home.module.css';

const Home = () => {
  return (
    <Layout.Content>
      <Carousel autoplay className={classes.homeSlider}>
        <div className={classes.mainSlide}>
          <img src={HomePict1} alt="Picture1" className={classes.sliderImg} />
          <div className={classes.sliderOverlay}>
            <p className={classes.sliderText}>
              Пласт – українська скаутська організація. Метою Пласту є сприяти всебічному, патріотичному вихованню та
              самовихованню української молоді на засадах християнської моралі. Гербом Пласту є трилиста квітка лілії –
              символ скаутського руху (відомий як Fleur-de-lis) – та тризуб, сплетені в одну гармонійну цілісність.
            </p>
          </div>
        </div>
        <div>
          <img src={HomePict2} alt="Picture1" className={classes.sliderImg} />
        </div>
        <div>
          <img src={HomePict3} alt="Picture1" className={classes.sliderImg} />
        </div>
      </Carousel>
      <div className={classes.cards__container}>
        <Card bordered={false}>
          <div className={classes.card__body}>
            <div className={classes.card__title}>
              <p>Чим ми займаємося?</p>
            </div>
            <div className={classes.card__text}>
              <p>
                Метою Пласту є сприяти всебічному патріотичному вихованню і самовихованню української молоді на засадах
                християнської моралі. Для досягнення цього ми діємо за пластовою методою, яка полягає у добровільності
                членства в організації, вихованні і навчання через гру та працю, поступовій програмі занять і
                випробувань, системі самоорганізації, житті серед природи, підтримці зацікавлень і здібностей дітей та
                молоді. Протягом року пластуни проводять понад 100 виховних таборів різної спеціалізації (летунський,
                морський, військовий, спортивний, мандрівний, виховно-вишкільний).
              </p>
            </div>
          </div>
        </Card>
        <Card bordered={false}>
          <div className={classes.card__body}>
            <div className={classes.card__title} style={{ order: 0 }}>
              <p>Де є осередки організації?</p>
            </div>
            <div className={classes.card__text}>
              <p>
                Пласт є найбільшою молодіжною організацією України: 137 осередок Пласту діє у 23 областях України. Пласт
                об’єднує 8500 пластунів різного віку – від наймолодших 2-річних пластунів до найстарших
                пластунів-сеньйорів, які допомагають у виховній праці. Також на сьогодні Пласт діє у 8 країнах світу
                серед української діаспори.
              </p>
            </div>
          </div>
        </Card>
        <Card bordered={false}>
          <div className={classes.card__body}>
            <div className={classes.card__title}>
              <p>Скільки років існує Пласт?</p>
            </div>
            <div className={classes.card__text}>
              <p>
                Вже 107 років існує Пласт, що з’явився як аналог скаутського руху в 1911 році. 12 квітня 1912 року у
                Львові вихованці одного з засновників Пласту, Олександра Тисовського, вперше склали Пластову Присягу. Цю
                дату традиційно вважають Днем народженням українського Пласту. Засновниками організації були Олександр
                Тисовський, Петро Франко та Іван Чмола.
              </p>
            </div>
          </div>
        </Card>
        <Card bordered={false}>
          <div className={classes.card__body}>
            <div className={classes.card__title} style={{ order: 0 }}>
              <p>Чи є вікові обмеження?</p>
            </div>
            <div className={classes.card__text}>
              <p>
                На відміну від більшості скаутських організацій світу, де членство завершується із досягненням 25-ліття,
                членство в Пласті є пожиттєвим. Умовний віковий поділ передбачає 4 вікові категорії: новаки (6-12
                років), юнаки (12-18 років), старші пластуни (18-35 років) та пластуні-сеніори (від 35 років). Виховна
                та адміністративна праця в Пласті здійснюється старшими пластунами та пластунами-сеніорами виключно на
                волонтерських засадах.
              </p>
            </div>
          </div>
        </Card>
      </div>
    </Layout.Content>
  );
};
export default Home;
