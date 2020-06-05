import React from 'react';
import { Form, Input, Button } from 'antd';
// @ts-ignore
import styles from './SignUp.module.css';
import Switcher from './Switcher/Switcher';
import { checkEmail, checkNameSurName } from './verification';

export default function () {
  const [form] = Form.useForm();

  const validationSchema = {
    email: [{ required: true, message: "Поле електронна пошта є обов'язковим" }, { validator: checkEmail }],
    password: [
      { required: true, message: "Поле пароль є обов'язковим" },
      { min: 6, message: 'Мінімальна допустима довжина - 6 символів' },
    ],
    name: [{ required: true, message: "Поле ім'я є обов'язковим" }, { validator: checkNameSurName }],
    surName: [{ required: true, message: "Поле прізвище є обов'язковим" }, { validator: checkNameSurName }],
    repeatedPassword: [
      { required: true, message: "Дане поле є обов'язковим" },
      { min: 6, message: 'Мінімальна допустима довжина - 6 символів' },
    ],
  };

  const onFinish = (values: any) => {
    console.log('Success:', values);
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log('Failed:', errorInfo);
  };

  const initialValues = {
    email: '',
    name: '',
    surName: '',
    password: '',
    repeatedPassword: '',
  };

  return (
    <div className={styles.mainContainer}>
      <Switcher page="SignUp" />
      <Form
        name="SignUpForm"
        initialValues={initialValues}
        form={form}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item name="email" rules={validationSchema.email}>
          <Input className={styles.SignUpInput} placeholder="Електронна пошта" />
        </Form.Item>
        <Form.Item name="password" rules={validationSchema.password}>
          <Input.Password visibilityToggle={false} className={styles.SignUpInput} placeholder="Пароль" />
        </Form.Item>
        <Form.Item
          name="repeatedPassword"
          dependencies={['password']}
          rules={[
            {
              required: true,
              message: 'Підтвердіть пароль',
            },
            ({ getFieldValue }) => ({
              validator(rule, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(new Error('Паролі не співпадають'));
              },
            }),
          ]}
        >
          <Input.Password visibilityToggle={false} className={styles.SignUpInput} placeholder="Повторіть пароль" />
        </Form.Item>
        <Form.Item name="name" rules={validationSchema.name}>
          <Input className={styles.SignUpInput} placeholder="Ім'я" />
        </Form.Item>
        <Form.Item name="surName" rules={validationSchema.surName}>
          <Input className={styles.SignUpInput} placeholder="Прізвище" />
        </Form.Item>
        <Form.Item>
          <Button htmlType="submit" id={styles.confirmButton}>
            Зареєструватись
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
}
