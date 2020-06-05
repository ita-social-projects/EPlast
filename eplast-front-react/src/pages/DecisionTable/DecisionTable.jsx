import React, { useEffect, useState } from 'react';
import { Table, Input, Button, Space, Layout } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import Highlighter from 'react-highlight-words';
// import columns from './columns';
import http from '../../api/http';
// import classes from './Table.module.css';

const { Content } = Layout;

const DecisionTable = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [searchedColumn, setSearchedColumn] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      const res = await http.get('todos');
      setData(res.data);
      setLoading(false);
    };
    fetchData();
  }, []);

  const handleSearch = (selectedKeys, confirm, dataIndex) => {
    confirm();
    setSearchText(selectedKeys[0]);
    setSearchedColumn(dataIndex);
  };

  const handleReset = (clearFilters) => {
    clearFilters();
    setSearchText('');
  };

  const itemRender = (current, type, originalElement) => {
    if (type === 'prev') {
      return <Button style={{ backgroundColor: '#3c5438', color: 'white' }}>Попередня</Button>;
    }
    if (type === 'next') {
      return <Button style={{ backgroundColor: '#3c5438', color: 'white' }}>Наступна</Button>;
    }
    return originalElement;
  };

  const getColumnSearchProps = (dataIndex) => ({
    // eslint-disable-next-line react/prop-types
    filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
      <div style={{ padding: 8 }}>
        <Input
          // ref={(node) => {
          //   this.searchInput = node;
          // }}
          placeholder={`Search ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={(e) => setSelectedKeys(e.target.value ? [e.target.value] : [])}
          onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
          style={{ width: 188, marginBottom: 8, display: 'block' }}
        />
        <Space>
          <Button
            type="primary"
            onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
            icon={<SearchOutlined />}
            size="small"
            style={{ width: 90 }}
          >
            Search
          </Button>
          <Button onClick={() => handleReset(clearFilters)} size="small" style={{ width: 90 }}>
            Reset
          </Button>
        </Space>
      </div>
    ),
    filterIcon: (filtered) => <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />,
    onFilter: (value, record) => record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()),
    render: (text) =>
      searchedColumn === dataIndex ? (
        <Highlighter
          highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
          searchWords={[searchText]}
          autoEscape
          textToHighlight={text.toString()}
        />
      ) : (
        text
      ),
  });

  const columns = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
    },
    {
      title: 'Назва',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Керівний орган',
      dataIndex: 'address',
      key: 'address0',
    },
    {
      title: 'Статус',
      dataIndex: 'completed',
      key: 'completed',
    },
    {
      title: 'Рішення для',
      dataIndex: 'userId',
      key: 'userId',
    },
    {
      title: 'Рішення',
      dataIndex: 'title',
      key: 'title',
      ...getColumnSearchProps('title'),
    },
    {
      title: 'Дата',
      dataIndex: 'address',
      key: 'adress1',
    },
    {
      title: 'Додатки',
      dataIndex: 'address',
      key: 'adress2',
    },
  ];

  return (
    <>
      {loading && (
        <Layout>
          <Content style={{ padding: '0 75px' }}>
            <h1 style={{ textAlign: 'center', marginTop: '20px' }}>Рішення керівних органів</h1>
            <Table loading columns={columns} />
          </Content>
        </Layout>
      )}
      {!loading && (
        <Layout>
          <Content style={{ padding: '0 75px' }}>
            <h1 style={{ textAlign: 'center', marginTop: '20px' }}>Рішення керівних органів</h1>
            <Table
              dataSource={data}
              columns={columns}
              rowKey="id"
              pagination={{
                itemRender,
                position: ['bottomCenter'],
                showTotal: (total, range) => `Записи з ${range[0]} по ${range[1]} із ${total} записів`,
              }}
            />
          </Content>
        </Layout>
      )}
    </>
  );
};

export default DecisionTable;
