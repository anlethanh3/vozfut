import { useState } from "react"
import { Button, Accordion, Form, FormGroup, FloatingLabel, OverlayTrigger, Tooltip, InputGroup } from "react-bootstrap";
import { FaSearch, FaTimesCircle } from "react-icons/fa";

const SearchMember = (props: { onSearchChanged: (props: { name: string }) => void, onSubmit: (props: { name: string }) => void }) => {
    const { onSearchChanged, onSubmit } = props

    const [name, setName] = useState('')

    const onChanged = (e: any) => {
        const value = e.target.value
        onSearchChanged({ name: value })
        setName(value)
    }

    const onSearch = (e: any) => {
        e.preventDefault();
        onSubmit({ name: name })
    }

    const onClear = () => {
        onSearchChanged({ name: '' })
        setName('')
    }

    return (
        <Accordion className="my-2" defaultActiveKey="0">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Search box</Accordion.Header>
                <Accordion.Body>
                    <Form onSubmit={onSearch}>
                        <FormGroup controlId="addMember.Name">
                            <InputGroup>
                                <InputGroup.Text><FaSearch/></InputGroup.Text>
                                <Form.Control type="text" placeholder="Input name" value={name} onChange={onChanged} />
                                <OverlayTrigger overlay={<Tooltip>Clear</Tooltip>}>
                                    <Button variant="danger" onClick={onClear}><FaTimesCircle /></Button>
                                </OverlayTrigger>
                            </InputGroup>
                        </FormGroup>
                    </Form>
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    )
}

export default SearchMember