import { Button, Modal, Row, Col, Card, Accordion } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { MemberProps } from '../slices/memberSlice';
import ReactCardFlip from 'react-card-flip';
import shuffle from 'lodash/shuffle'
import AddMemberClass, { AddClassMemberProps } from './AddMemberClass';
import { MatchProps } from '../slices/matchSlice';
import { RollingProps } from '../slices/matchDetailSlice';

interface FlipMemberProps {
    isFlip: boolean,
    member: MemberProps,
    teamId: number,
}

interface FlipMemberState {
    isShowAdd: boolean,
    data: FlipMemberProps[][],
    teamId: number,
}

export default function FlipMember(props: { show: boolean, match: MatchProps, initial: MemberProps[], onSubmit: (data: RollingProps[]) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, initial, match } = props
    const [state, setState] = useState<FlipMemberState>({ data: [[], [], []], isShowAdd: false, teamId: 0 })
    const onValid = () => {
        let list: RollingProps[] = []
        for (let index = 0; index < match.teamCount; index++) {
            let list1: MemberProps[] = []
            let sum = 0
            state.data.forEach(i => {
                i.filter(x => x.teamId === index).forEach(i => {
                    sum += i.member.elo
                    list1.push({ id: i.member.id, name: i.member.name, elo: i.member.elo })
                })
            })
            list.push({ eloSum: sum, players: list1 })
        }
        onSubmit(list)
    }

    const onFlipItem = (data: FlipMemberProps) => {
        data.isFlip = true
        data.teamId = state.teamId
        state.teamId += 1
        if (state.teamId >= match.teamCount) {
            state.teamId = 0
        }
        setState({ ...state })
    }

    useEffect(() => {
        var list: FlipMemberProps[][] = [[], [], []]
        initial.forEach((i) => {
            list[2].push({ isFlip: false, member: i, teamId: -1 })
        })
        let slist = shuffle(list[2])
        list[2] = slist
        setState({ ...state, data: list, isShowAdd: false, })
    }, [initial])

    function drawGrid(members: FlipMemberProps[]): JSX.Element[] {
        var colItems: JSX.Element[] = [];
        var rowItems: JSX.Element[] = [];
        var col = 4
        var row = members.length / col
        for (let i = 0; i < row; i++) {
            for (let j = 0; j < col; j++) {
                let num = (i * col) + j
                if (num >= members.length) { continue; }
                let data = members[num]
                let member = members[num].member
                let isFlip = members[num].isFlip
                let item = (
                    <Col className='pb-2'>
                        <ReactCardFlip isFlipped={isFlip} flipDirection="vertical">
                            <Card style={{ height: '8em', cursor: 'pointer' }} onClick={() => { onFlipItem(data) }}>
                                <Card.Body>
                                    <Card.Title>{num + 1}</Card.Title>
                                </Card.Body>
                            </Card>
                            <Card style={{ height: '8em' }} >
                                <Card.Body>
                                    <Card.Title>{member.name}</Card.Title>
                                    <Card.Subtitle>Elo: +{member.elo}</Card.Subtitle>
                                </Card.Body>
                                <Card.Footer>Team: {data.teamId + 1}</Card.Footer>
                            </Card>
                        </ReactCardFlip>
                    </Col>
                )
                colItems.push(item)
            }
            rowItems.push((<Row>{colItems}</Row>))
            colItems = []
        }
        return rowItems
    }

    function drawTabs(): JSX.Element {
        return (
            <Accordion defaultActiveKey="2">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Class SSS</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[0])
                        }
                    </Accordion.Body>
                </Accordion.Item>
                <Accordion.Item eventKey="1">
                    <Accordion.Header>Class S</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[1])
                        }
                    </Accordion.Body>
                </Accordion.Item>
                <Accordion.Item eventKey="2">
                    <Accordion.Header>Class A</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[2])
                        }
                    </Accordion.Body>
                </Accordion.Item>
            </Accordion>
        )
    }

    function setShowAdd(isShow: boolean) {
        setState({ ...state, isShowAdd: isShow })
    }

    function onAddMemberClass(data: AddClassMemberProps) {
        let array = initial.filter(i => data.memberIds.includes(i.id))
        if (array.length <= 0) {
            return
        }

        let list: FlipMemberProps[][] = []
        state.data.forEach(i => {
            list.push(i.filter(k => !data.memberIds.includes(k.member.id)))
        })
        array.forEach(i => {
            list[data.classId].push({ isFlip: false, member: i, teamId: -1 })
        })
        list[data.classId] = shuffle(list[data.classId])
        setState({ ...state, isShowAdd: false, data: list })
    }

    return (<>
        {
            state.isShowAdd &&
            <AddMemberClass members={initial} onSubmit={(data) => { onAddMemberClass(data) }} onClose={() => { setShowAdd(false) }} show={state.isShowAdd} />
        }
        <Modal show={show} fullscreen size='xl' onHide={onClose} backdrop="static">
            <Modal.Header closeButton>
                <Modal.Title>Flip Member: Turn Team {state.teamId + 1} pick</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    drawTabs()
                }
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant='info' onClick={() => { setShowAdd(true) }}>Add Member to Class</Button>
                <Button variant="primary" onClick={onValid}>
                    Save Team Squad
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}