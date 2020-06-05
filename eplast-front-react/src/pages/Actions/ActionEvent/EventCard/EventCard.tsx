import React from 'react';
// import { useHistory } from "react-router-dom";
import 'antd/dist/antd.css';
import { Card} from 'antd';
import { EditOutlined, EllipsisOutlined, SettingOutlined } from '@ant-design/icons';

const classes = require('./EventCard.module.css');

interface CardProps {
    title: string;
    name: string;
    imgUrl: string;
    userId: string;
    id: string;
}

interface Props {
    item: CardProps;
}

const EventCard = ({
    item: { name},
}: Props) => {
    const { Meta } = Card;
    // const history = useHistory();

    return (
        <div className={classes.background}>
            <div className={classes.actionsWrapper}>
                <Card
                className={classes.cardStyles}
                    cover={
                        <img
                            alt="example"
                            src="https://www.nicepng.com/png/detail/75-750003_handshake-comments-shaking-hands-icon.png"
                        />
                    }
                    actions={[
                        <SettingOutlined key="setting" />,
                        <EditOutlined key="edit" />,
                        <EllipsisOutlined key="ellipsis" />,
                    ]}
                >
                    <Meta
                        title={name}
                        description="This is the description"
                    />
                </Card>
            </div>
        </div>
    )
}
export default EventCard;